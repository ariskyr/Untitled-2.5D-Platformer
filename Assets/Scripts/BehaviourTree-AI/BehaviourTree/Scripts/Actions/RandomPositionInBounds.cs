using UnityEngine;
using BehaviourTree;

public class RandomPositionInBounds : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.target.x = Random.Range(context.boundingBox.BoundsMin.x, context.boundingBox.BoundsMax.x);
        blackboard.target.z = Random.Range(context.boundingBox.BoundsMin.z, context.boundingBox.BoundsMax.z);
        return State.Success;
    }
}
