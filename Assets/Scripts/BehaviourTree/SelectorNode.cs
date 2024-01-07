using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    private int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Success:
                return State.Success;
            case State.Failure:
                current++;
                break;
        }

        return current == children.Count ? State.Failure : State.Success;
    }
}
