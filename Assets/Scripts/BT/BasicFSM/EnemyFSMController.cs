using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSMController
{
    public FSMBase CurrentState { get; private set; }

    public void ChangeState(FSMBase newState)
    {
        if (CurrentState == null || newState.Priority > CurrentState.Priority)
        {

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        else
        {
            Debug.Log($"⚠️ FSM BLOCK: Cannot Change from [{CurrentState.Priority}] to [{newState.Priority}].");
        }
    }
    public void ForceChangeState(FSMBase newState)
    {
        Debug.Log($"💥 FSM FORCE: Change from [{CurrentState?.GetType().Name ?? "None"}] to [{newState.GetType().Name}]");

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void Reset()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }
}
