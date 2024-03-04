using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    private Vector2 crouchMovementVelocity;
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetTopColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetTopColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        {
            crouchMovementVelocity = playerData.crouchMovementVelocity * moveInput;
            player.SetVelocityXZ(crouchMovementVelocity);
            player.CheckIfShouldFlip(moveInput.x);

            if(moveInput.Equals(Vector2.zero))
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
