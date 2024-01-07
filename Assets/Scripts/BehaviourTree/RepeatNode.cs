using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    public int count = 1;
    public bool loopForever = false;
    public bool endOnFailure = false;

    private int currentCount = 0;

    protected override void OnStart()
    {
        currentCount = 0;
    }

    protected override void OnStop()
    {
        currentCount = 0;
    }

    protected override State OnUpdate()
    {
        //if loopforever, ignore child status and keep looping
        if (loopForever)
        {
            State childState = child.Update();            
            //if endOnFailure is true, stop repeat, and fail the node
            if (childState == State.Failure && endOnFailure)
            {
                return State.Failure;
            }
            return State.Running;

        }

        while (currentCount < count)
        {
            State childState = child.Update();

            //if endOnFailure is true, stop repeat, and fail the node
            if (childState == State.Failure && endOnFailure)
            {
                return State.Failure;
            }
            else if (childState == State.Success || childState == State.Failure)
            { 
                currentCount++;

                //if the desired count is reached, return success
                if (currentCount >= count && !loopForever)
                {
                    return State.Success;
                }
            }
            else
            {
                return State.Running;
            }
        }
        return State.Running;
    }
}
