using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class RandomWait : ActionNode {
    //pick duration between min and max
    public float durationMin = 2f;
    public float durationMax = 5f;
    private float duration;
    private float startTime;

    protected override void OnStart() {
        context.agent.speed = 0;
        startTime = Time.time;
        duration = Random.Range(durationMin, durationMax);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
