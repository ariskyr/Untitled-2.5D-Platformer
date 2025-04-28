using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState2D : PlayerGroundedState2D
{
    private float movementVelocity;

    public PlayerMoveState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Moving player
        player.CheckIfShouldFlip(moveInput.x);

        movementVelocity = playerData.movementVelocity * moveInput.x;

        if (!isExitingState)
        {
            player.SetVelocityX(movementVelocity);
            if (moveInput.x == 0f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (crouchInput)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}