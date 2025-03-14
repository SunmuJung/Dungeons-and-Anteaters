using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float speed, damage, playerDetectionRange,
                                   attackRange, attackRadious, attackCoolDown,
                                   rigorCoolDown; 
    [SerializeField] protected Transform attackSpot;
    [SerializeField] protected string playerTag;
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected State enemyState;
    private Animator animator;
    protected Transform targetPoint, playerPosition;
    protected int currentPatrolPoint;
    public bool isFacingRight, canAttack, rigorStarted;

    public enum State {
        Idle,
        Chasing,
        Attacking,
        Rigor,
        Dead
    }

    public virtual void Start(){
        enemyState = State.Idle;
        targetPoint = patrolPoints[currentPatrolPoint];
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    public virtual void Update()
    {
        switch (enemyState)
        {
            case State.Idle:
                targetPoint = patrolPoints[currentPatrolPoint];
                animator.SetBool("Move", false);
                if (DetectPlayer(playerDetectionRange))
                {
                    enemyState = State.Chasing;
                }
                break;
            case State.Chasing:
                targetPoint = playerPosition;
                Movement();
                animator.SetBool("Move", true);
                if (DetectPlayer(attackRange))
                {
                    enemyState = State.Attacking;
                }
                break;
            case State.Attacking:
                if (canAttack)
                {
                    Attack();
                    StartCoroutine(AttackCoolDown());
                }
                if (!DetectPlayer(playerDetectionRange))
                {
                    enemyState = State.Idle;
                }
                else if (!DetectPlayer(attackRange))
                {
                    enemyState = State.Chasing;
                }
                break;
            case State.Rigor:
                if (!rigorStarted)
                {
                    StartCoroutine(RigorCoolDown());
                }
                break;
            case State.Dead:
                break;
        }
    }

    //Checks if the player is at a certain distance away from the enemy.
    public virtual bool DetectPlayer(float radious){
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radious);
        foreach (Collider2D target in collisions){
            if(target?.gameObject.tag == playerTag){
                playerPosition = target.transform;
                return true;
            }
        }
        return false;
    }

    public virtual void Attack()
    {
        // Collider2D hit = Physics2D.OverlapCircle(attackSpot.position, attackRadious);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadious);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Health>()?.Damage(10); // 플레이어에게 10의 데미지를 추가로 줌
            }
        }
    }

    //Moves the enemy to the patrol location in the array if it has reached it,
    //it should switch tot he next one.
    public virtual void Movement(){
        if(Vector2.Distance(transform.position, targetPoint.position) <= attackRange){
            if(currentPatrolPoint == patrolPoints.Length-1){
                currentPatrolPoint = 0;
            }
            else{
                currentPatrolPoint++;
            }
            targetPoint = patrolPoints[currentPatrolPoint];
        }

        Vector2 dir = targetPoint.position - transform.position;

        if(isFacingRight && dir.x < 0f || !isFacingRight && dir.x > 0f)
        {
            Flip();
        }
        transform.position += new Vector3(dir.x, 0f,0f).normalized * speed * Time.deltaTime;
    }

    //Flips the sprite whenever the player changes direction.
    public void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    public virtual IEnumerator AttackCoolDown(){
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    public virtual IEnumerator RigorCoolDown(){
        rigorStarted = true;
        State temp  = enemyState;
        yield return new WaitForSeconds(rigorCoolDown);
        rigorStarted = false;
        enemyState = temp;
    }
}
