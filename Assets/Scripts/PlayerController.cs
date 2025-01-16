using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float xMinClamp = -41.5f;
    [SerializeField] private float xMaxClamp =  47.1f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float playerFallTimer = 0f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float sphereCastRadius = 0.2f;

    private PlayerControls inputActions;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector2 movement;
 
    private bool isGrounded = true;
    private float playerYpos;



    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Move.performed += Move_performed;
        rb = GetComponent<Rigidbody>();
        playerYpos = transform.position.y;
       
     
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        movement = obj.ReadValue<Vector2>();
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if(isGrounded)
        {
            
            velocity.y = jumpForce;
        }
    }


    private bool IsGrounded()
    {

        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayLength = 1f;
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
        {
            isGrounded = true;
            
            return true;
        }
        else
        {
            
            isGrounded = false;
        }
      
        return false;
    }

  
   

    private void FixedUpdate()
    {
      
        ApplyGravityAndVelocity();
      
    }


    private void Update()
    {
        // Ground Check
        isGrounded = IsGrounded();

        if(velocity.y < 0f && isGrounded)
        {
            velocity.y = 0f;
            
        } 
       
    }

    private float PlayerGravity()
    {
        if (isGrounded)
        {
            gravity = 0f;
            
        }
        else
        {
            playerFallTimer -= Time.fixedDeltaTime;
            if(playerFallTimer < 0)
            {
                gravity = -20f;
            }
            playerFallTimer = 0f;
        }

        return gravity;
    }

    private void ApplyGravityAndVelocity()
    {
        // Apply gravity if not grounded manually when kinematic
        if (!isGrounded)
        {
            velocity.y += PlayerGravity() * Time.deltaTime;
        }
        else if (velocity.y < 0f) 
        {
            velocity.y = 0f;
        }

        // Update position based on velocity (for both horizontal and vertical movement)
        Vector3 newPosition = rb.position + new Vector3(movement.x * moveSpeed * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime, 0f);
        newPosition.x = Mathf.Clamp(newPosition.x, xMinClamp, xMaxClamp);

        rb.MovePosition(newPosition);
        
    }



    private void OnDestroy()
    {
        inputActions.Player.Disable();
    }


}
