using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState2D : PlayerState2D
{
    protected Vector2 moveInput;
    private bool jumpInput;
    protected bool crouchInput;
    protected bool attackInput;

    private bool isGrounded;

    public PlayerGroundedState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        jumpInput = InputManager.Instance.GetJumpPressed();
        crouchInput = InputManager.Instance.GetCrouchPressed();
        attackInput = InputManager.Instance.GetAttackPressed();

        /*if (attackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else*/ if (jumpInput)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
