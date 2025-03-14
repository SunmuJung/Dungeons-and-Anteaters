//Coder: Brandon Retana
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Playables;
using UnityEngine.UIElements;

//This class is in charge on managing the movement of the player; back and forth, jumping, and dashing.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed, jumpHeight, playerHeigth,
                                   dashForce, dashCoolDown, dashTime;
    [SerializeField] private int additionalJumpCount;
    [SerializeField] private int jumpCount;
    [SerializeField] private TrailRenderer tail;
    [SerializeField] private LayerMask layer;

    private PlayerStatus status;
    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Animator animator;
    private bool hasDashed, facingRight = true;
    private Vector2 playerDirection;
    private Vector2 prevPos;
    private Vector2 currPos;
    //Property only read tells if the player is facing right.
    public bool FacingRight{get{return facingRight;}}

    //Use New Input System by creating an instance then subscribing methods to the events.
    private void Awake()
    {
        animator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
        currPos = transform.position;
        prevPos = currPos;

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
        status.isAir = !OnGround();
        updateSpeed();

        if (status.isAttacking)
        {
            playerControls.Player.Movement.Disable();
            playerControls.Player.Jump.Disable();
            playerControls.Player.Dash.Disable();
        }
        else
        {
            playerControls.Player.Movement.Enable();
            playerControls.Player.Jump.Enable();
            playerControls.Player.Dash.Enable();
        }
    }

    private void updateSpeed()
    {
        float observedSpeedX = Mathf.Abs((currPos.x - prevPos.x) / Time.deltaTime);
        // Debug.Log(observedSpeedX);
        // playerCurrentSpeedX = observedSpeedX;
        if (status.isWalking != (observedSpeedX > 0.1f))
        {
            status.isWalking = observedSpeedX > 0.1f;
            animator.SetBool("isWalking", status.isWalking);
        }
        prevPos = currPos;
        currPos = transform.position;
    }

    private bool IsGrounded()
    {
        
        Collider2D underPlayer = Physics2D.Raycast(transform.position, Vector2.down, playerHeigth + 0.3f, layer).collider;
        Debug.Log(underPlayer);
        return underPlayer != null ? underPlayer.tag == "Ground" : false;
    }

    

    private bool OnGround()
    {
        return IsGrounded();// && Mathf.Abs(rb.velocity.y) <= 0.01f;
    }

    private void GroundCheck()
    {
        if (OnGround()) jumpCount = 0;
    }

    //Adds impulsive force upwards on the player eveytime the player is over a "steppable" object.
    private void Jump(InputAction.CallbackContext context)
    {
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
        // Emits a nice trail behind the player
        // tail.emitting = true;
        playerControls.Player.Movement.Disable();

        yield return new WaitForSecondsRealtime(length);

        rb.gravityScale = gravity;
        rb.velocity = Vector2.zero;
        // Stops emitting a nice trail behind the player
        // tail.emitting = false;
        playerControls.Player.Movement.Enable();

        yield return new WaitForSecondsRealtime(coolDown);
        hasDashed = false;
    }
}
