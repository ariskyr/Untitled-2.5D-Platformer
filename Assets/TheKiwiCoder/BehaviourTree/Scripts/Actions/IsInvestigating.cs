using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsInvestigating : Conditional
{
    [SerializeField] private float MinAwarenessToInvestigate = 1f;

    protected override State OnUpdate() {
        //no targets
        if (context.awarenessSystem.ActiveTargets == null || context.awarenessSystem.ActiveTargets.Count == 0)
            return base.OnUpdate();

        //if awareness is between 1 and 2, investigate
        foreach (var candidate in context.awarenessSystem.ActiveTargets.Values)
        {
            if (candidate.Awareness >= MinAwarenessToInvestigate && blackboard.wasChasing)
            {
                blackboard.target = candidate.RawPosition;
                return State.Success;
            }
        }
        blackboard.wasChasing = false;
        return State.Failure;
    }
}
