using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{ 
    public event Action<int> onExperienceGained;
    public void ExperienceGained(int experience)
    {
        if (onExperienceGained != null)
        {
            onExperienceGained(experience);
        }
    }

    public event Action<int> onLevelUp;
    public void LevelUp(int level)
    {
        if (onLevelUp != null)
        {
            onLevelUp(level);
        }
    }
}
