using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Attack : ActionNode
{
    private Vector3 direction;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        //set eye direction
        if (context.sensorStats)
        {
            direction = context.agent.velocity.normalized;
            context.sensorStats.EyeDirection = direction;
        }

        //set animation
        context.animator.SetFloat("DirectionX", direction.x);
        context.animator.SetFloat("DirectionY", direction.z);


        //attack
        context.attackPoint.Attack();
       
        return State.Success;
    }
}
