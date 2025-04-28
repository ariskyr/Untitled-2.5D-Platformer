using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D
{
    protected Player2D player;
    protected PlayerStateMachine2D stateMachine;
    protected PlayerData playerData;

    protected float startTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    private string animBoolName;

    public PlayerState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Animator.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishedTrigger()
    {
        isAnimationFinished = true;
    }
}
