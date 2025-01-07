using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private LayerMask groundLayer;

    Vector2 movement;
    Rigidbody rb;
    float jump;
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
        if (!context.started) return;
         
     
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessJump();
        
    }

    private void ProcessMovement()
    {
        if (movement != null)
        {
            Vector3 currentPosition = rb.position;
            Vector3 moveVector = new Vector3(movement.x, 0f, 0f);
            rb.MovePosition(currentPosition + moveVector * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void ProcessJump()
    {
        if (!IsGrounded()) return;

        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
    }

    private void Update()
    {
       
    }

    private bool IsGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, groundLayer))
        {
            return true;
        }

        return false;
    }
}
