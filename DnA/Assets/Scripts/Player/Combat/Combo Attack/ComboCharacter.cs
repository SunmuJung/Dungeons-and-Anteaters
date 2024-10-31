using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{

    private StateMachine meleeStateMachine;

    [SerializeField] public GameObject hitbox;
    [SerializeField] public GameObject Hiteffect;

    private PlayerStatus status;
    private Animator anim;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!status.isAttacking && (Input.GetKeyDown("x") || Input.GetMouseButton(0) ) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            meleeStateMachine.SetNextState(new GroundEntryState());
        }

        hitbox.SetActive(anim.GetFloat("Weapon.Active") > 0.01f ? true : false);
    }
}
