using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private ConstantForce2D cf;
    private float movementX;
    private float movementY;
    public float moveSpeed;
    
    // Jumping Variables
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpBrake;
    [SerializeField] float mass;
    [SerializeField] float linearDrag;
    [SerializeField] float angularDrag;
    [SerializeField] float gravity;

    private InputAction inputAction;

    void Start()
    {
        updateValues();
    }
    private void FixedUpdate()
    {
        Move(movementX);
        checkYVelocity();
        
    }
    public void onUpdateValues(InputAction.CallbackContext ctx) 
    {
        if (ctx.performed) 
        {
            updateValues();
        }
    }
    public void updateValues()
    {
        rb = GetComponent<Rigidbody2D>();
        cf = GetComponent<ConstantForce2D>();
        rb.mass = mass;
        rb.drag = linearDrag;
        rb.angularDrag = angularDrag;
        rb.gravityScale = gravity;
    }
    

    // OnMove is called with the movement button is pressed
    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementX = ctx.ReadValue<Vector2>().x;
        //Debug.Log(moveDirection);
    }

    // Move is called on Update and controls actual direction
    // direction is the Vector2 from the Callback Context (ctx)
    public void Move(float direction)
    {
        //Debug.Log(direction);
        rb.velocity = new Vector2(movementX * moveSpeed, rb.velocity.y);
        //Debug.Log(rb.velocity);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) 
        {
            Jump();
            Debug.Log("Jump!");
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //Debug.Log("Grounded");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }

    public void checkYVelocity() 
    {
        if (rb.velocity.y < .5 && !isGrounded)
        {
            cf.force = new Vector2(0, -jumpBrake);
        }
        else if(isGrounded)
        {
            cf.force = new Vector2(0, 0); 
        }
        //Debug.Log(cf.force);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //6 is Ground
        if (collision.gameObject.layer == 6) 
        {
            isGrounded = true;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        //6 is Ground
        if (collision.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }
    
}