using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RandomPositionAroundTarget : ActionNode
{
    [SerializeField] private float investigationRange = 2f;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.target = blackboard.target + Random.insideUnitSphere * investigationRange;
        return State.Success;
    }
}
