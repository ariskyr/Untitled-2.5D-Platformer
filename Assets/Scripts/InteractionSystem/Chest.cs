using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Open Chest";

    [Header("Loot")]
    [SerializeField] private int minGold = 5;
    [SerializeField] private int maxGold = 10;

    public string InteractionPrompt => GetInteractionPrompt();

    private int _guardingNPCs = 0;
    private bool _isOpened = false;



    public bool Interact(Interactor interactor)
    {
        if (_isOpened) return false;
        if (_guardingNPCs > 0) return false;

        OpenChest();

        return true;
    }

    private void OpenChest()
    {
        _isOpened = true;
        int goldGained = Random.Range(minGold, maxGold + 1);
        GameEventsManager.Instance.playerEvents.GoldGained(goldGained);

    }

    private string GetInteractionPrompt()
    {
        if (_isOpened) return "Empty";
        if (_guardingNPCs > 0) return "Enemies are nearby";
        //default
        return _prompt;
    }

    // Called by child GuardTrigger
    public void NPCEntered()
    {
        _guardingNPCs++;
    }

    // Called by child GuardTrigger
    public void NPCExited()
    {
        _guardingNPCs = Mathf.Max(0, _guardingNPCs - 1);
    }
}
