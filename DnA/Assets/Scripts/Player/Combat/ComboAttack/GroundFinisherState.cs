using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 3;
        duration = 0.5f;
        anim.SetTrigger("Attack" + attackIndex);
        Debug.Log("Player Attack" + attackIndex + " Fired");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedTime >= duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
