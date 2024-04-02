using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionAnimator : MoveToPosition
{

    private Vector3 direction;
    protected override void OnStart()
    {
        base.OnStart();
               

    }

    protected override void OnStop()
    {
        base.OnStop();
    }

    protected override State OnUpdate()
    {

        direction = context.agent.velocity.normalized;
        //set eye direction
        if (context.sensorStats)
        {
            
            context.sensorStats.EyeDirection = direction;
        }

        //set animation
        context.animator.SetFloat("DirectionX", direction.x);
        context.animator.SetFloat("DirectionY", direction.z);

        return base.OnUpdate();
    }
}
