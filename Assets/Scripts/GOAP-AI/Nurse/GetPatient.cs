using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    //this resource is atm the cubicle
    GameObject resource;
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatient();
        if (target == null)
        {
            return false;
        }
        resource = GWorld.Instance.RemoveCubicle();
        if (resource != null)
        {
            inventory.AddItem(resource);
        }
        else
        {
            GWorld.Instance.AddPatient(target);
            target = null;
            return false;
        }

        GWorld.Instance.GetWorld().ModifyState("freeCubicle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("patientWaiting", -1);
        if (target)
            target.GetComponent<GAgent>().inventory.AddItem(resource);
        return true;
    }
}
