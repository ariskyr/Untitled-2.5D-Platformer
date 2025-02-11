using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private Color originalColor;
    private SpriteRenderer playerSprite;

    public PlayerDeadState(Player player, PlayerStateMachine stateMachine,
        PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    public override void Enter()
    {
        base.Enter();

        originalColor = playerSprite.color;

        // Stop all movement and physics
        player.SetVelocityXZ(Vector2.zero);
        player.RB.velocity = Vector3.zero;
        player.RB.isKinematic = true; // Optional: Prevent physics interactions

        //red tint
        playerSprite.color = new Color(1, 0.5f, 0.5f, 1);

    }

    public override void Exit()
    {
        playerSprite.color = originalColor;

        base.Exit();
        player.RB.isKinematic = false; // Restore physics if needed
    }

    public override void LogicUpdate()
    {
        // Prevent any state transitions
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        // Prevent physics updates
        base.PhysicsUpdate();
    }

    public override void AnimationFinishedTrigger()
    {
        // Optional: Handle animation completion if needed
        base.AnimationFinishedTrigger();
    }
}