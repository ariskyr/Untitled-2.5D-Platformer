using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInAirState2D : PlayerState2D
{
    private Vector2 moveInput;
    private float movementVelocity;
    private bool isGrounded;

    public PlayerInAirState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        if (isGrounded && player.CurrentVelocity.y <= 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else
        {
            //Moving while in air
            player.CheckIfShouldFlip(moveInput.x);

            movementVelocity = playerData.movementVelocity * moveInput.x;
            player.SetVelocityX(movementVelocity);

            player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
