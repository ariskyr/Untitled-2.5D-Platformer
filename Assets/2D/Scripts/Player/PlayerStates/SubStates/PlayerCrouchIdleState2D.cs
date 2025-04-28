using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState2D : PlayerGroundedState2D
{
    public PlayerCrouchIdleState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            if (!moveInput.Equals(Vector2.zero))
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (!crouchInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
