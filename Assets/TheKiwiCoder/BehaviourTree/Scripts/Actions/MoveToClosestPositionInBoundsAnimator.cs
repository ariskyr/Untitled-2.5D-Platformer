using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToClosestPositionInBoundsAnimator : MoveToPositionAnimator
{
    public float offset = 0.3f;

    protected override void OnStart()
    {
        base.OnStart();
        context.agent.destination = GetClosestPositionToBounds(context.transform.position);
    }

    private Vector3 GetClosestPositionToBounds(Vector3 position)
    {
        float x = Mathf.Clamp(position.x, context.boundingBox.BoundsMin.x +offset, context.boundingBox.BoundsMax.x -offset);
        float z = Mathf.Clamp(position.z, context.boundingBox.BoundsMin.z +offset, context.boundingBox.BoundsMax.z -offset);

        return new Vector3(x, context.transform.position.y, z);
    }
}
