using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class RandomPositionInRangeOfObject : ActionNode
{
    public float range = 5f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.pointOfInterest)
        {
            blackboard.target = blackboard.pointOfInterest.transform.position + Random.insideUnitSphere * range;
            return State.Success;
        }
        else 
            return State.Failure;
    }
}
