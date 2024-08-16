using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITest : MonoBehaviour 
{
    protected EnemyState currentState = EnemyState.Idle;

    public float detectRange = 5f;
    public float attackRange = 1f;
    public float rigorTime = 5f;
    public int health = 100;

    [SerializeField]
    private float followingSpeed = 2f;

    [SerializeField]
    private float idleSpeed = 1f;

    [SerializeField]
    private float chaseToIdleDelay = 2f; // Delay before transitioning from Chasing to Idle

    private float chaseTimer = 0f;
    private float rigorTimer = 0f;
    private GameObject player;
    private Renderer enemyRenderer;

    // Variables to store dynamic idle movement points
    private Vector2 pointA;
    private Vector2 pointB;
    //private Vector2 lastPosition; // To remember where the enemy last stopped when leaving the chasing state

    //private bool isChasingPlayer = false;

    protected enum EnemyState
    {
        Idle,
        MoveTowardsPlayer,
        Attack,
        Halt,
        Rigor,
        Dead
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRenderer = GetComponent<Renderer>(); // Get the Renderer component
        UpdateEnemyColor(); // Set the initial color based on the initial state

        // Initialize pointA and pointB based on the starting position
        UpdateIdlePoints(transform.position);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.MoveTowardsPlayer:
                MoveTowardsPlayerState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Halt:
                HaltState();
                break;
            case EnemyState.Rigor:
                RigorState();
                break;
            case EnemyState.Dead:
                DeadState();
                break;
        }

        CheckTransitions();
    }

    void IdleState()
    {
        // Move back and forth between dynamically set points
        float pingPongValue = Mathf.PingPong(Time.time * idleSpeed, 1);
        Vector2 targetPosition = Vector2.Lerp(pointA, pointB, pingPongValue);

        // Flip the enemy to face the direction of movement
        if (targetPosition.x > transform.position.x)
        {
            // Moving to the right, face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (targetPosition.x < transform.position.x)
        {
            // Moving to the left, face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Move the enemy
        transform.position = targetPosition;
        UpdateEnemyColor();  // Ensure color changes to reflect the state
    }

    void MoveTowardsPlayerState()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Move the enemy towards the player at a consistent speed
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, followingSpeed * Time.deltaTime);

            // Flip the enemy to face the direction of the player
            if (direction.x > 0)
            {
                // Moving to the right, face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                // Moving to the left, face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            // Update the last position as it moves
            //lastPosition = transform.position;
        }

        UpdateEnemyColor();  // Ensure color changes to reflect the state
    }

    void AttackState()
    {
        // Logic for attacking the player
        UpdateEnemyColor();

    }

    void HaltState()
    {
        // Logic for staying still
        UpdateEnemyColor();

    }

    void RigorState()
    {
        rigorTimer += Time.deltaTime;
        if (rigorTimer >= rigorTime)
        {
            currentState = EnemyState.Idle;
            rigorTimer = 0f;
            UpdateIdlePoints(transform.position);  // Update idle points to current position when leaving Rigor
        }
        UpdateEnemyColor();

    }

    void DeadState()
    {
        // Logic for when the enemy is dead
        UpdateEnemyColor();

    }

    void CheckTransitions()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Check if the enemy is dead
        if (health <= 0)
        {
            currentState = EnemyState.Dead;
            return;
        }

        // Transitions based on the FSM diagram
        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToPlayer <= detectRange && distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.MoveTowardsPlayer;
                    //isChasingPlayer = true;
                }
                else if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                break;

            case EnemyState.MoveTowardsPlayer:
                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                else if (distanceToPlayer > detectRange)
                {
                    //isChasingPlayer = false; //Stop enemy from moving
                    chaseTimer += Time.deltaTime;
                    if (chaseTimer >= chaseToIdleDelay)
                    {
                        chaseTimer = 0f;
                        UpdateIdlePoints(transform.position);  // Update idle points to current position when stopping chase
                        currentState = EnemyState.Idle;  // Transition back to idle
                        
                    }
                }
                break;

            case EnemyState.Attack:
                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.MoveTowardsPlayer;
                }
                break;

            case EnemyState.Halt:
                if (distanceToPlayer <= detectRange)
                {
                    currentState = EnemyState.MoveTowardsPlayer;
                }
                break;

            case EnemyState.Rigor:
                if (rigorTimer >= rigorTime)
                {
                    currentState = EnemyState.Idle;
                    UpdateIdlePoints(transform.position);  // Update idle points to current position when leaving Rigor
                }
                break;
        }
    }
    // Function to update pointA and pointB based on the current position
    void UpdateIdlePoints(Vector2 currentPosition)
    {
        float offset = 3f;  // The distance the enemy will move back and forth
        pointA = new Vector2(currentPosition.x - offset, currentPosition.y);
        pointB = new Vector2(currentPosition.x + offset, currentPosition.y);
    }
    void UpdateEnemyColor()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                enemyRenderer.material.color = Color.white;  // Idle state color
                break;
            case EnemyState.MoveTowardsPlayer:
                enemyRenderer.material.color = Color.yellow;  // Chasing state color
                break;
            case EnemyState.Attack:
                enemyRenderer.material.color = Color.red;  // Attacking state color
                break;
            case EnemyState.Halt:
                enemyRenderer.material.color = Color.blue;  // Halt state color
                break;
            case EnemyState.Rigor:
                enemyRenderer.material.color = Color.gray;  // Rigor state color
                break;
            case EnemyState.Dead:
                enemyRenderer.material.color = Color.black;  // Dead state color
                break;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            currentState = EnemyState.Dead;
        }
    }
}
