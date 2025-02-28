using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : EnemyAI
{

    private Animator animator;
    private Healthbar healthbar;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>(); // 애니메이터 초기화
        healthbar = FindObjectOfType<Healthbar>(); // Healthbar 인스턴스 초기화
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (DetectPlayer(attackRange))
        {
            Attack();
        }
    }

    public override bool DetectPlayer(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(playerTag))
            {
                playerPosition = hitCollider.transform;
                return true;
            }
        }
        return false;
    }

    public override void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack"); // 공격 애니메이션 실행
            StartCoroutine(DealDamage());
            StartCoroutine(AttackCoolDown());
        }
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션이 실행되는 시간 대기
        if (Vector2.Distance(transform.position, playerPosition.position) <= attackRange)
        {
            PlayerStatus playerStatus = playerPosition.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                healthbar.takeDamage(10); // 플레이어에게 10의 데미지 입히기
                Debug.Log("RAT ATTACKED PLAYER");
            }
        }
    }
    public override void Movement()
    {
        base.Movement();
    }
}
