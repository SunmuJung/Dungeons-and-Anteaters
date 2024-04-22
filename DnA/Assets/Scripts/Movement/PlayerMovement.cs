//Coder: Brandon Retana
using UnityEngine;
using System.Collections;

//This class is in charge on managing the movement of the player; back and forth, jumping, and dashing.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed, jumpHeight, dashForce, dashCoolDown, dashTime;
    [SerializeField] private LayerMask ground;
    [SerializeField] private TrailRenderer tail;

    private Rigidbody2D rb;
    private InputManager inputManager;
    private bool hasDashed, isDashing, facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = InputManager.Instance;
    }

    //Updates movement of the player every frame.
    private void Update()
    {
        if (!isDashing)
        {
            Movement();

            if (rb.IsTouchingLayers(ground) && inputManager.PlayerJumpedThisFrame())
            {
                Jump();
            }

            if (!hasDashed && inputManager.PlayerDashedThisFrame())
            {
                Dash();
            }
        }
    }

    //Flips the sprite whenever the player changes direction.
    private void Flip()
    {
        if (facingRight && inputManager.GetPlayerMovement().x < 0f || !facingRight && inputManager.GetPlayerMovement().x > 0f)
        {
            Vector2 scale = rb.transform.localScale;
            scale.x *= -1f;
            rb.transform.localScale = scale;
            facingRight = !facingRight;
        }
    }

    //Moves the player getting input information from the input manager.
    private void Movement()
    {
        rb.velocity = new Vector2(inputManager.GetPlayerMovement().x * playerSpeed, rb.velocity.y);
        Flip();
    }

    //Jumps using input information from the input manager.
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    //Makes the player dash using a coroutine.
    private void Dash()
    {
        StartCoroutine(DashTimer(dashCoolDown, dashTime));
    }

    //Makes the player dash but it cannot move while dashing.
    IEnumerator DashTimer(float coolDown, float length)
    {
        hasDashed = true;
        isDashing = true;
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);

        //Emits a nice trail behind the player
        tail.emitting = true;

        yield return new WaitForSecondsRealtime(length);
        rb.gravityScale = gravity;

        //Stops emitting a nice trail behind the player
        tail.emitting = false;

        isDashing = false;
        yield return new WaitForSecondsRealtime(coolDown);
        hasDashed = false;
    }
}
