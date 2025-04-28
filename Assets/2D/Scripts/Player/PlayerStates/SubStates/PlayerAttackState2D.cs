using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState2D : PlayerAbilityState2D
{
    public PlayerAttackState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
        player.attackPoint.Attack();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //player.SetVelocityXZ(Vector2.zero);

    }
    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        isAbilityDone = true;
    }
}
