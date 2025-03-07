using System.Linq.Expressions;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] public string customName;
    [SerializeField] private State mainStateType;
    [SerializeField] public State CurrentState { get; private set; }
    [SerializeField] private State nextState;

    private PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();

        SetNextStateToMain();
    }


    // Update is called once per frame
    void Update()
    {


        if (nextState != null && !status.isBasicAttacking)
        {
            SetState(nextState);
        }

        if (CurrentState != null)
            CurrentState.OnUpdate();
    }

    private void SetState(State _newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    private void LateUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}