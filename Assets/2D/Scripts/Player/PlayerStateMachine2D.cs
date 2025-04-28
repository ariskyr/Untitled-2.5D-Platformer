using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine2D
{
    public PlayerState2D CurrentState { get; private set; }

    public void Initialize(PlayerState2D startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState2D newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
