using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Death : ActionNode
{
    protected override void OnStart() {
        if (context.agent != null)
        {
            context.agent.isStopped = true;
        }

        if (context.boxCollider != null)
        {
            context.boxCollider.enabled = false;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // Wait for death animation to finish
        if (context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(context.gameObject, 2f); // Optional delay before destruction
            return State.Success;
        }
        return State.Running;
    }
}
