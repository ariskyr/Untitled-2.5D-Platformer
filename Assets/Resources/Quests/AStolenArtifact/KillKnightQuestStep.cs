using UnityEngine;

public class KillKnightQuestStep : QuestStep
{
    private bool knightKilled = false;
    private string knightName = "Blackguard Knight";

    private void Start()
    {
        GameEventsManager.Instance.miscEvents.onKnightKilled += OnKnightKilled;
        UpdateState();
    }

    private void OnKnightKilled(string knight_name)
    {
        if (!knightKilled && IsTargetKnight(knight_name))
        {
            knightKilled = true;
            UpdateState();
            FinishQuestStep();
        }
    }

    private bool IsTargetKnight(string name)
    {
        // Your knight identification logic here
        if (name == knightName)
        { 
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateState()
    {
        string state = knightKilled ? "1" : "0";
        string status = knightKilled ? 
            $"{knightName} defeated" : 
            $"Slay {knightName}";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        try
        {
            knightKilled = state == "1";
            UpdateState();
        }
        catch
        {
            Debug.LogError("Failed to parse knight kill state: " + state);
            knightKilled = false;
            UpdateState();
        }
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.miscEvents.onKnightKilled -= OnKnightKilled;
    }
}