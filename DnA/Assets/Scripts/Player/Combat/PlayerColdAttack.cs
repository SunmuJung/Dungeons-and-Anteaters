using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColdAttack : MonoBehaviour
{
    [SerializeField] private GameObject spear;
    [SerializeField] private Transform pos;
    [SerializeField] private float cooltime;
    [SerializeField] public GameObject Canvas;
    private Animator animator;
    private PlayerControls playerInput;
    private PlayerMovement playerMovement;
    private bool canFire = true;

    private PlayerStatus status;
    private SkillCoolDown cooldownUI;


    private void Awake(){
        animator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
        cooldownUI = Canvas.GetComponentInChildren<SkillCoolDown>();
        cooldownUI.cooldownTime[0] = cooltime;

        playerInput = new();
        playerInput.Player.ColdSkill.performed += ColdSkillAttack;
        OnEnable();
    }

    void Start(){
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnEnable()
    {
        playerInput.Player.ColdSkill.Enable();
    }

    public void OnDisable()
    {
        playerInput.Player.ColdSkill.Disable();
    }

    private void ColdSkillAttack(InputAction.CallbackContext context)
    {
        if (status.isAttacking) return;

        if (canFire)
        {
            status.isSkill = true;
            cooldownUI.StartCooldown(0);
            animator.SetTrigger("FrostSkill");
            StartCoroutine(ColdCoolDown(cooltime));
            StartCoroutine("generateSpear");
        }
    }

    IEnumerator generateSpear()
    {
        yield return new WaitForSecondsRealtime(0.55f);
        GameObject projectile = Instantiate(spear, pos.position + new Vector3(0, 0.5f), transform.rotation);
        Vector2 dir = playerMovement.FacingRight ? Vector2.right : Vector2.left;
        
        projectile.GetComponent<spear>().Direction = dir;
    }

    IEnumerator ColdCoolDown(float time)
    {
        canFire = false;
        yield return new WaitForSeconds(time);
        canFire = true;
    }
}
