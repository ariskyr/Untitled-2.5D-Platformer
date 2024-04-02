using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : GenericSingleton<QuestManager>, IDataPersistence
{
    private Dictionary<string, Quest> questMap;

    private int currentPlayerLevel;

    protected override void Awake()
    {
        base.Awake();
        questMap = CreateQuestMap();
    }

    public void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.Instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
    }

    public void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.Instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
    }

    public void LoadData(GameData data)
    {
        List<string> questIDs = new List<string>(questMap.Keys);

        foreach (string questID in questIDs)
        {
            try
            {
                // get the corresponding quest object from questMap
                Quest quest = questMap[questID];
                Quest loadedQuest = null;
                //load quest from saved data
                if (data.questData.ContainsKey(quest.info.id))
                {
                    QuestData questData = data.questData[quest.info.id];
                    loadedQuest = new Quest(quest.info, questData.state, questData.questStepIndex, questData.questStepStates);
                }
                //therwise, load quest with default state
                else
                {
                    loadedQuest = new Quest(quest.info);
                }

                //Add or update the loaded quest in the questMap
                questMap[questID] = loadedQuest;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load quest with id " + questID + ": " + e);
            }

        }

        //broadcast the initial state of all quests
        foreach (Quest quest in questMap.Values)
        {
            //initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    public void SaveData(GameData data)
    {
        data.questData.Clear();
        foreach (Quest quest in questMap.Values)
        {
            try
            {
                QuestData questData = quest.GetQuestData();
                //!!! be very careful with this serialization
                string serializedQuestData = JsonUtility.ToJson(questData);
                data.questData.Add(quest.info.id, questData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
            }
        }
    }
    
    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        //check if the player level is high enough
        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

        //check prerequisites
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        //move to next
        quest.MoveToNextStep();
        //instantiate the next step if it exists
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        //if there's no next step, finish the quest
        else
        {
            ChangeQuestState(id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        GameEventsManager.Instance.playerEvents.ExperienceGained(quest.info.expReward);
        GameEventsManager.Instance.playerEvents.GoldGained(quest.info.goldReward);
    }

    private void QuestStepStateChange(string questId, int stepIndex, QuestStepState state)
    {
        Quest quest = GetQuestById(questId);
        quest.StoreQuestStepState(state, stepIndex);
        //broadcast the new state of the quest
        ChangeQuestState(questId, quest.state); 
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        //Load all QuestInfoSO objects under Assets/Resources/Quests folder
        //be careful, the naming of the folder is case sensitive
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate quest id found: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogWarning("Quest not found for id: " + id);
        }
        return quest;
    }
}
