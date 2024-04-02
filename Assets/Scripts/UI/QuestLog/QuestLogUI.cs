using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private TextMeshProUGUI goldRewardsText;
    [SerializeField] private TextMeshProUGUI expRewardsText;
    [SerializeField] private TextMeshProUGUI levelRequirementsText;
    [SerializeField] private TextMeshProUGUI questRequirementsText;

    private Button firstSelectedButton;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void Update()
    {
        if (InputManager.Instance.GetQuestTogglePressed())
        {
            if (contentParent.activeInHierarchy)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
    }

    private void ShowUI()
    {
        contentParent.SetActive(true);
        //disable player movement possibly

        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    private void HideUI()
    {
        contentParent.SetActive(false);
        //enable player movement possibly

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void QuestStateChange(Quest quest)
    {
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () =>
        {
            SetQuestLogInfo(quest);
        });

        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }

        //set color
        questLogButton.SetState(quest.state);
    }

    private void SetQuestLogInfo(Quest quest)
    {
        //name
        questDisplayNameText.text = quest.info.questName;

        //status
        questStatusText.text = quest.GetFullStatusText();

        //rewards
        goldRewardsText.text = quest.info.goldReward + " Gold";
        expRewardsText.text = quest.info.expReward + " Exp";

        //requirements
        levelRequirementsText.text = "Level: " + quest.info.levelRequirement;
        questRequirementsText.text = "";
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            questRequirementsText.text += prerequisiteQuestInfo.questName + "\n";
        }
    }
}
