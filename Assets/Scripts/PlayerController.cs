using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float xMinClamp = -41.5f;
    [SerializeField] private float xMaxClamp =  47.1f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float gravity = -9.8f;


    private Rigidbody rb;

    private float verticalVelocity = 0f; // Custom vertical velocity


  
    private Vector2 movement;
   

    private bool isGrounded = true;
  

   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxJumpHeight = transform.position.y + maxJumpHeight;
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
       
    }
   
    public void JumpInput(InputAction.CallbackContext context)
    {
   
        if (context.performed && IsGrounded())
        {

            isGrounded = false;
            StartCoroutine(ProcessJump());
           
            Debug.Log("Jump executed!");
        }

        
       
    }

    IEnumerator ProcessJump()
    {
        verticalVelocity = jumpForce;
        // Simulate jump arc until peak height is reached
        while (verticalVelocity > 0 || !isGrounded)
        {

             verticalVelocity += gravity * Time.deltaTime; // Apply custom gravity
           
          
           
            transform.Translate(new Vector3(0, verticalVelocity * Time.deltaTime, 0));

            if (IsGrounded() && verticalVelocity < 0)
            {
                verticalVelocity = 0f; // Reset velocity when landing
                break;
            }

            yield return null;
        }

        

    }
    
    private bool IsGrounded()
    {
      

        if (Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer))
        {
            isGrounded = true;
            
            Debug.Log("Player is grounded!");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Player is in the air!");
        }
      
        return isGrounded;
    }

    private void ProcessMovement()
    {
        if (movement != null)
        {
            Vector3 currentPosition = rb.position;
            Vector3 moveVector = new Vector3(movement.x, 0f, 0f);
            Vector3 newPosition = currentPosition + moveVector * moveSpeed * Time.fixedDeltaTime;

            //clamp player
            newPosition.x = Mathf.Clamp(newPosition.x, xMinClamp, xMaxClamp);

            rb.MovePosition(newPosition);
        }
    }




    private void Update()
    {
        // Update grounded state
        isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
         ProcessMovement();  
    }

    
}
