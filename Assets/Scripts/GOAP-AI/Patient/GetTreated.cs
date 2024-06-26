using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("cubicle");
        if (target == null)
        {
            return false;
        }

        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("isTreated", 1);
        agentBeliefs.ModifyState("isCured", 1);
        inventory.RemoveItem(target);
        return true;
    }
}
