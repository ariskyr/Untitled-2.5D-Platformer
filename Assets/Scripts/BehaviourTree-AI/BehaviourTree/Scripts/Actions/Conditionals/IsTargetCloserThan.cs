using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class IsTargetCloserThan : Conditional
{
    [SerializeField] private float attackRange = 1f;

    protected override State OnUpdate() {
        
        float dist = Vector3.Distance(context.transform.position, blackboard.target);
        if (dist < attackRange)
        {
            _isTriggered = true;
        }
        else
        {
            _isTriggered = false;
        }
        return base.OnUpdate();
    }
}
