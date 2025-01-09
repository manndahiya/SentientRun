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
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private static float globalGravity = -9.81f;

 

    private Rigidbody rb;

   
    private Vector2 movement;

    private float velocity;
    private bool isGrounded = true;

  
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
     
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
            
           
            Debug.Log("Jump executed!");
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
        velocity += globalGravity * gravityScale * Time.deltaTime;

        if (isGrounded)
        {
            velocity = 0f;
        }

         if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = jumpForce;

            }
            
        
        transform.Translate(Vector3.up * velocity * Time.deltaTime);

    }

    private void FixedUpdate()
    {
         ProcessMovement();  
    }

    
}
