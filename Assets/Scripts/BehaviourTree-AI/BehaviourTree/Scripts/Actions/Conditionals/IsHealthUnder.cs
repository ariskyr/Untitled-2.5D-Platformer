using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class IsHealthUnder : Conditional
{
    [SerializeField] private float healthThreshold = 10f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        _isTriggered = context.damageable.CurrentHealth <= healthThreshold;

        return base.OnUpdate();
    }
}
