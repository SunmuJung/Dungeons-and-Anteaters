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
    protected State enemyState;
    protected Transform targetPoint, playerPosition;
    protected int currentPatrolPoint;
    protected bool isFacingRight, canAttack, rigorStarted;

    protected enum State {
        Idle,
        Chasing,
        Attacking,
        Rigor,
        Dead
    }

    private void Update()
    {
        switch (enemyState){
            case State.Idle:
                targetPoint = patrolPoints[currentPatrolPoint];
                Movement();
                if(DetectPlayer(playerDetectionRange)){
                    enemyState = State.Chasing;
                }
                break;
            case State.Chasing:
                targetPoint = playerPosition;
                Movement();
                if(DetectPlayer(attackRange)){
                    enemyState = State.Attacking;
                }
                else if (!DetectPlayer(playerDetectionRange)){
                    enemyState = State.Idle;
                }
                break; 
            case State.Attacking:
                if(canAttack){
                    Attack();
                    StartCoroutine(AttackCoolDown());
                }
                if(!DetectPlayer(playerDetectionRange)){
                    enemyState = State.Idle;
                }
                else if (!DetectPlayer(attackRange)){
                    enemyState = State.Chasing;
                }
                break;
            case State.Rigor:
                if(!rigorStarted){
                    StartCoroutine(RigorCoolDown());
                }
                break;
        }
    }

    protected virtual void Start(){
        enemyState = State.Idle;
        targetPoint = patrolPoints[currentPatrolPoint];
        canAttack = true;
    }

    //Checks if the player is at a certain distance away from the enemy.
    protected virtual bool DetectPlayer(float radious){
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radious);
        foreach (Collider2D target in collisions){
            if(target?.gameObject.tag == playerTag){
                playerPosition = target.transform;
                return true;
            }
        }
        return false;
    }

    protected virtual void Attack(){
        Collider2D hit = Physics2D.OverlapCircle(attackSpot.position, attackRadious);
        hit?.GetComponent<Health>()?.Damage(damage);
    }

    //Moves the enemy to the patrol location in the array if it has reached it,
    //it should switch tot he next one.
    protected virtual void Movement(){
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
    protected void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    protected virtual IEnumerator AttackCoolDown(){
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    protected virtual IEnumerator RigorCoolDown(){
        rigorStarted = true;
        State temp  = enemyState;
        yield return new WaitForSeconds(rigorCoolDown);
        rigorStarted = false;
        enemyState = temp;
    }
}
