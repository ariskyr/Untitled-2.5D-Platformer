using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class IsTargetDetected : Conditional
{
    [SerializeField] private float MinAwarenessToChase = 2f;
    protected override State OnUpdate() {
        //no targets to chase
        if (context.awarenessSystem.ActiveTargets == null || context.awarenessSystem.ActiveTargets.Count == 0)
            return base.OnUpdate();

        //acquire a new target if possible
        foreach (var candidate in context.awarenessSystem.ActiveTargets.Values)
        {
            if (candidate.Awareness >= MinAwarenessToChase)
            {
                blackboard.target = candidate.RawPosition;
                blackboard.wasChasing = true;
                return State.Success;
            }
        }

        return State.Failure;
    }
}
