using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityXZ(Vector2.zero);
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

   /* public void Attacking()
    {
        Collider[] hitcolliders = Physics.OverlapSphere(player.attackPoint.position, playerData.attackRange);
        foreach (Collider enemy in hitcolliders)
        {
            if (enemy.gameObject.tag == "Enemy" && enemy.GetComponent<CharacterStats>() != null)
            {
                //playerCombat item contains the stats of the player (attack damage etc) and reduces the stats of the enemy (currentHealth)
                //playerCombat.Attack(enemy.GetComponent<CharacterStats>());
            }
        }
    }*/
}
