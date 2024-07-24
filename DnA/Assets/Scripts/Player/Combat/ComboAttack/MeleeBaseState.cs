using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    // the least time durtaion the state takes before moving on to the next state
    public float duration;
    protected Animator anim;
    // should the next combo played?
    protected bool shouldCombo;
    protected int attackIndex;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        anim = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // should be changed to another input key.
        if (Input.GetMouseButtonDown(0))
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
