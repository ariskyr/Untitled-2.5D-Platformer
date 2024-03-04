using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace TheKiwiCoder
{
    public class LogVariable : ActionNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            Debug.Log("Log node: "+ DialogueManager.Instance.DialogueIsPlaying);
            return State.Success;
        }
    }
}
