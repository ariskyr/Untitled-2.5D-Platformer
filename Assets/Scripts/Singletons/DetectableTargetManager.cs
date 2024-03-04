using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableTargetManager : GenericSingleton<DetectableTargetManager>
{
    public List<DetectableTarget> AllTargets { get; private set; } = new List<DetectableTarget>();


    public void Register(DetectableTarget target)
    {
        AllTargets.Add(target);
    }

    public void Deregister(DetectableTarget target)
    {
        AllTargets.Remove(target);
    }
}
