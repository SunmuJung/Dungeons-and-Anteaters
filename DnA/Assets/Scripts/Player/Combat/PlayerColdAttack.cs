using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColdAttack : MonoBehaviour
{
    [SerializeField] private GameObject spear;
    [SerializeField] private Transform pos;
    [SerializeField] private float cooltime;
    private bool CanShoot = true;
    private Animator animator;
    private PlayerControls playerInput;
    private PlayerMovement playerMovement;

    private PlayerStatus status;

    private void Awake(){
        animator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>(); 

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
        
        if (CanShoot){
            status.isSkill = true;
            animator.SetTrigger("FrostSkill");
            StartCoroutine("generateSpear");

            StartCoroutine(cooldown());
        }
    }

    IEnumerator generateSpear()
    {
        yield return new WaitForSecondsRealtime(0.55f);
        GameObject projectile = Instantiate(spear, pos.position + new Vector3(0, 0.5f), transform.rotation);
        Vector2 dir = playerMovement.FacingRight ? Vector2.right : Vector2.left;
        
        projectile.GetComponent<spear>().Direction = dir;
    }

    IEnumerator cooldown(){
        CanShoot = false;
        yield return new WaitForSeconds(cooltime);
        CanShoot = true;
    }

    IEnumerator OnAnimationEnd()
    {
        status.isSkill = false;
        yield break;
    }
}
