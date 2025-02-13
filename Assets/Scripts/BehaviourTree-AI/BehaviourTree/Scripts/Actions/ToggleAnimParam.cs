using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Linq;

public class ToggleAnimParam : ActionNode
{
    [SerializeField]
    private bool _idle = true;
    [SerializeField]
    private  bool _move = false;
    [SerializeField]
    private bool _attack  = false;
    [SerializeField] 
    private bool _dead = false;

    private readonly Dictionary<string, bool> parameterMapping = new();

    protected override void OnStart() {

        //reset all params
        foreach (AnimatorControllerParameter param in context.animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                context.animator.SetBool(param.name, false);
            }
        }

        //Map bool params
        //TODO: probably there exists a better implementation of this
        //but I am too dumb
        parameterMapping.Clear();
        parameterMapping.Add("Idle", _idle);
        parameterMapping.Add("Move", _move);
        parameterMapping.Add("Attack", _attack);
        parameterMapping.Add("Dead", _dead);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (SanityCheck(_idle, _move, _attack, _dead) != 1)
        {
            Debug.LogWarning("Tried to set more than one bool as true");
            return State.Failure;
        }
        //set the chosen one to true
        foreach (var kvp in parameterMapping)
        {
            if (kvp.Value)
            {
                context.animator.SetBool(kvp.Key, true);
                break;
            }
        }

        return State.Success;
    }

    //checks if more than one value was set to true
    private static int SanityCheck(params bool[] booleans)
    {
        return booleans.Count(b => b);
    }
}
