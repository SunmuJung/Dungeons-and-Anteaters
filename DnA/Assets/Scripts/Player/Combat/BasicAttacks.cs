//Coder: Brandon
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttacks : MonoBehaviour
{
    [SerializeField] private float damage, radious, range;
    [SerializeField] private LayerMask attackLayer;
    private PlayerControls combatControls;
    //Variable to access the playerMovement Script.
    private PlayerMovement playerMovement;

    private void Start()
    {
        //Only enable the input actions that will be used in the script.
        combatControls = new();
        combatControls.Player.BasicAttack.performed += BasicAttack;
        combatControls.Player.BasicAttack.Enable();
        //Gets the PlayerMovement Script in the player.
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnDisable(){
        combatControls.Player.BasicAttack.Disable();
    }

    private void BasicAttack(InputAction.CallbackContext context){
        //Checks the direction the player is facing.
        Vector2 dir = Vector2.left;
        if(playerMovement.FacingRight){
            dir = Vector2.right;
        }
        //Casts a circle and does damage to any objects in that layer with a health script.
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radious, dir, range, attackLayer);
        hit.collider?.GetComponent<Health>().Damage(damage);
    }
}
