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

    public event Action<int> onPlayerLevelChange;
    public void PlayerLevelChange(int level)
    {
        if (onPlayerLevelChange != null)
        {
            onPlayerLevelChange(level);
        }
    }

    public event Action<int> onPlayerExperienceChange;
    public void PlayerExperienceChange(int experience)
    {
        if (onPlayerExperienceChange != null)
        {
            onPlayerExperienceChange(experience);
        }
    }

    public event Action<int> onGoldGained;
    public void GoldGained(int gold)
    {
        if (onGoldGained != null)
        {
            onGoldGained(gold);
        }
    }

    public event Action<int> onGoldChange;
    public void GoldChange(int gold)
    {
        if (onGoldChange != null)
        {
            onGoldChange(gold);
        }
    }

    public event Action<int> onPlayerHealthChange;
    public void PlayerHealthChange(int health)
    {
        if (onPlayerHealthChange != null)
        {
            onPlayerHealthChange(health);
        }
    }

    public event Action<int> onHealthGained;
    public void HealthGained(int health)
    {
        if (onHealthGained != null)
        {
            onHealthGained(health);
        }
    }

    public event Action<int> onHealthLost;
    public void HealthLost(int health)
    {
        if (onHealthLost != null)
        {
            onHealthLost(health);
        }
    }
}
