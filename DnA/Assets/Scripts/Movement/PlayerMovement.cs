//Coder: Brandon Retana
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

//This class is in charge on managing the movement of the player; back and forth, jumping, and dashing.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed, jumpHeight, playerHeigth,
                                   dashForce, dashCoolDown, dashTime;
    [SerializeField] private TrailRenderer tail;
    [SerializeField] private LayerMask layer;

    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private bool hasDashed, facingRight = true;
    private Vector2 playerDirection;

    //Use New Input System by creating an instance then subscribing methods to the events.
    private void Awake()
    {
        playerControls = new();
        playerControls.Player.Movement.started += Movement;
        playerControls.Player.Movement.performed += Movement;
        playerControls.Player.Movement.canceled += Movement;
        playerControls.Player.Jump.started += JumpStarted;
        playerControls.Player.Dash.started += Dash;
        playerControls.Enable();
    }

    //Gets the rigid body of the player.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Helper method to disble the movement system.
    private void OnDisable()
    {
        playerControls.Disable();
    }

    //Updates movement of the player every frame.
    private void Update()
    {
        transform.position += (Vector3)playerDirection * Time.deltaTime;
    }

    //Adds impulsive force upwards on the player eveytime the player is over a "steppable" object.
    private void JumpStarted(InputAction.CallbackContext context)
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, playerHeigth, layer).collider != null)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpHeight), ForceMode2D.Impulse);
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
        Vector2 scale = rb.transform.localScale;
        scale.x *= -1f;
        rb.transform.localScale = scale;
        facingRight = !facingRight;
    }

    //Makes the player dash using a coroutine.
    private void Dash(InputAction.CallbackContext context)
    {
        if (!hasDashed)
        {
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

        yield return new WaitForSecondsRealtime(length);
        rb.gravityScale = gravity;

        //Stops emitting a nice trail behind the player
        tail.emitting = false;

        yield return new WaitForSecondsRealtime(coolDown);
        hasDashed = false;
    }
}
