using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : GenericSingleton<PlayerUI>
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onPlayerHealthChange += PlayerHealthChange;
        GameEventsManager.Instance.playerEvents.onPlayerExperienceChange += PlayerExperienceChange;
        GameEventsManager.Instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
        GameEventsManager.Instance.playerEvents.onGoldChange += GoldChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onPlayerHealthChange -= PlayerHealthChange;
        GameEventsManager.Instance.playerEvents.onPlayerExperienceChange -= PlayerExperienceChange;
        GameEventsManager.Instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
        GameEventsManager.Instance.playerEvents.onGoldChange -= GoldChange;
    }

    private void PlayerHealthChange(int health)
    {
        hpSlider.value = (float)health / (float)100;
    }

    private void PlayerExperienceChange(int experience)
    {
        xpSlider.value = (float)experience / (float)100;
    }

    private void PlayerLevelChange(int level)
    {
        levelText.text = "Level: " + level;
    }

    private void GoldChange(int gold)
    {
        goldText.text = gold + " Gold";
    }
}
