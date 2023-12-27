using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private Vector3 workspace;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        /*DoChecks();
        player.Animator.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);*/
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.CheckIfShouldFlip(input.x);

        workspace = playerData.movementVelocity * input;
        player.setVelocityXZ(workspace.x, workspace.z);

        if (input.Equals(Vector3.zero))
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
