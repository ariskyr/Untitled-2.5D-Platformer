using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState2D : PlayerGroundedState2D
{
    private float crouchMovementVelocity;

    public PlayerCrouchMoveState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.SetTopColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        //player.SetTopColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            crouchMovementVelocity = playerData.crouchMovementVelocity * moveInput.x;
            player.SetVelocityX(crouchMovementVelocity);
            player.CheckIfShouldFlip(moveInput.x);

            if (moveInput.x == 0f)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if (!crouchInput)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
