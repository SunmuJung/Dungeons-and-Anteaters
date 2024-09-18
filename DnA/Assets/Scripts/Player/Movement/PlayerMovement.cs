//Coder: Brandon Retana
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

//This class is in charge on managing the movement of the player; back and forth, jumping, and dashing.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed, jumpHeight, playerHeigth,
                                   dashForce, dashCoolDown, dashTime;
    [SerializeField] private int additionalJumpCount;
    [SerializeField] private int jumpCount;
    [SerializeField] private TrailRenderer tail;
    [SerializeField] private LayerMask layer;

    private Rigidbody2D rb;
    private PlayerControls playerControls;
    protected Animator animator;
    private bool hasDashed, facingRight = true;
    private Vector2 playerDirection;
    //Property only read tells if the player is facing right.
    public bool FacingRight{get{return facingRight;}}

    //Use New Input System by creating an instance then subscribing methods to the events.
    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerControls = new();
        playerControls.Player.Movement.started += Movement;
        playerControls.Player.Movement.performed += Movement;
        playerControls.Player.Movement.canceled += Movement;
        playerControls.Player.Jump.started += Jump;
        playerControls.Player.Dash.started += Dash;
        OnEnable();
    }

    //Gets the rigid body of the player.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable() {
        playerControls.Player.Movement.Enable();
        playerControls.Player.Jump.Enable();
        playerControls.Player.Dash.Enable();
    }

    //Helper method to disble the movement system.
    private void OnDisable()
    {
        playerControls.Player.Movement.Disable();
        playerControls.Player.Jump.Disable();
        playerControls.Player.Dash.Disable();
    }

    //Updates movement of the player every frame.
    private void Update()
    {
        transform.position += (Vector3)playerDirection * Time.deltaTime;
        GroundCheck();
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, playerHeigth + 0.1f, layer).collider != null;
    }

    private bool OnGround()
    {
        return IsGrounded() && Mathf.Abs(rb.velocity.y) <= 0.01f;
    }

    private void GroundCheck()
    {
        if (OnGround()) jumpCount = 0;
    }

    //Adds impulsive force upwards on the player eveytime the player is over a "steppable" object.
    private void Jump(InputAction.CallbackContext context)
    {
        // if (animator.is)
        animator.SetTrigger("Jump");

        
        if (IsGrounded())
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector2(rb.velocity.x, jumpHeight), ForceMode2D.Impulse);
        }
        else
        {
             if (jumpCount < additionalJumpCount)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector2(rb.velocity.x, jumpHeight), ForceMode2D.Impulse);
                ++jumpCount;
            }
        }
    }

    //Gets the values for movement and saves it on a vector, then it flips the sprite of the player if necessary.
    private void Movement(InputAction.CallbackContext context)
    {
        playerDirection = context.ReadValue<Vector2>() * playerSpeed;
        if(facingRight && playerDirection.x < 0f || !facingRight && playerDirection.x > 0f)
        {
            Flip();
        }
    }

    //Flips the sprite whenever the player changes direction.
    private void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    //Makes the player dash using a coroutine.
    private void Dash(InputAction.CallbackContext context)
    {
        if (!hasDashed)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(DashTimer(dashCoolDown, dashTime));
        }
    }

    //Makes the player dash but it cannot move while dashing.
    IEnumerator DashTimer(float coolDown, float length)
    {
        hasDashed = true;
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        //Emits a nice trail behind the player
        tail.emitting = true;
        playerControls.Player.Movement.Disable();

        yield return new WaitForSecondsRealtime(length);

        rb.gravityScale = gravity;
        rb.velocity = Vector2.zero;
        //Stops emitting a nice trail behind the player
        tail.emitting = false;
        playerControls.Player.Movement.Enable();

        yield return new WaitForSecondsRealtime(coolDown);
        hasDashed = false;
    }
}
