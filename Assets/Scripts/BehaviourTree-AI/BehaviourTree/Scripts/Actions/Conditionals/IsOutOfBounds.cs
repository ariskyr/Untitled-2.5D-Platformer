using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOutOfBounds : Conditional
{
    protected override State OnUpdate()
    {
        if (context.transform.position.x < context.boundingBox.BoundsMin.x ||
            context.transform.position.x > context.boundingBox.BoundsMax.x ||
            context.transform.position.z < context.boundingBox.BoundsMin.z ||
            context.transform.position.z > context.boundingBox.BoundsMax.z)
        {
            _isTriggered = true;
        }
        
        return base.OnUpdate();
    }
}
