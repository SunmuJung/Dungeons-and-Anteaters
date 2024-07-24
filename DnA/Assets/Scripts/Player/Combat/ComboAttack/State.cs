using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }

    public StateMachine stateMachine;

    // Called at the beginning to set initial variables
    public virtual void OnEnter(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    // Called every frame to update the state
    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }
    public virtual void OnFixedUpdate()
    {
        fixedTime += Time.deltaTime;
    }
    public virtual void OnLateUpdate()
    {
        lateTime += Time.deltaTime;
    }

    // Called at the end to clean up any data
    public virtual void OnExit()
    {

    }
}
