using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Conditional : ActionNode
{
    protected bool _isTriggered = false;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (_isTriggered)
        {
            _isTriggered = false;
            return State.Success;
        }
        return State.Failure;
    }
}
