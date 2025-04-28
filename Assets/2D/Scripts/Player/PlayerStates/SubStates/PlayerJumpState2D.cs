using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState2D : PlayerAbilityState2D
{
    public PlayerJumpState2D(Player2D player, PlayerStateMachine2D stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true; //Jump state instantaneously changes to inAir or idle state
    }
}
