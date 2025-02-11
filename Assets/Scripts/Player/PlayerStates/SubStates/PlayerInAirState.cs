using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private Vector2 moveInput;
    private Vector2 movementVelocity;
    private bool isGrounded;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
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

        moveInput = InputManager.Instance.GetMovePressed();

        if (isGrounded && player.CurrentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isGrounded && Math.Abs(player.CurrentVelocity.y) > 0.000001f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            //Moving while in air
            player.CheckIfShouldFlip(moveInput.x);

            movementVelocity = playerData.movementVelocity * moveInput;
            player.SetVelocityXZ(movementVelocity);

            player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
