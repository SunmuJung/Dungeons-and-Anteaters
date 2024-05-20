using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class FireSkill : MonoBehaviour
{
    [SerializeField] private GameObject FireFist;
    [SerializeField] private Transform pos;
    [SerializeField] private float cooldown = 2f;
    private PlayerControls combatControls;
    private bool canFireBall = true;

    private void Start()
    {
        // Start the skill logic when the skill is activated

        //Only enable the input actions that will be used in the script.
        combatControls = new();
        combatControls.Player.fireSkill.performed += fireSkill;
        combatControls.Player.fireSkill.Enable();
        //Gets the PlayerMovement Script in the player.

    }

   

    private void OnDisable()
    {
        combatControls.Player.fireSkill.Disable();
    }

    private void fireSkill(InputAction.CallbackContext context)
    {
        if (canFireBall)
        {
            StartCoroutine(FireCoolDown(cooldown));
            var FireFIst = Instantiate(FireFist, pos.position, pos.rotation);
        }
    }

    IEnumerator FireCoolDown(float time)
    {
        canFireBall = false;
        yield return new WaitForSeconds(time);
        canFireBall = true;
    }

}
    