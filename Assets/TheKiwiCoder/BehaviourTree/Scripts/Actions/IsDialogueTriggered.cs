using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsDialogueTriggered : Conditional
{
    protected override State OnUpdate()
    {
        if (DialogueManager.Instance.DialogueIsPlaying)
        {
            _isTriggered = true;
        }

        return base.OnUpdate();
    }

}
