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
    private PlayerControls playerInput;
    private PlayerMovement playerMovement;

    private void Awake(){
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
        if(CanShoot){
            GameObject projectile = Instantiate(spear, pos.position, transform.rotation);
            Vector2 dir = Vector2.left;
            if(playerMovement.FacingRight){
                dir = Vector2.right;
            }
            projectile.GetComponent<spear>().Direction = dir;
            StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown(){
        CanShoot = false;
        yield return new WaitForSeconds(cooltime);
        CanShoot = true;
    }
}
