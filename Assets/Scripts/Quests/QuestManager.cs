using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : GenericSingleton<QuestManager>
{
    private Dictionary<string, Quest> questMap;

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
    }

    public void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        //broadcast the initial state of all quests
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void StartQuest(string id)
    {
        Debug.Log("Starting quest with id: " + id);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advancing quest with id: " + id);
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Finishing quest with id: " + id);
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
