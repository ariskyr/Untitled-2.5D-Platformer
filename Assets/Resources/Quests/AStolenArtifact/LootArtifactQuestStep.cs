using UnityEngine;

public class LootArtifactQuestStep : QuestStep
{
    private bool artifactLooted = false;
    private string artifactName = "Sacred Chalice";

    private void Start()
    {
        GameEventsManager.Instance.miscEvents.onArtifactCollected += OnArtifactCollected;
        UpdateState();
    }

    private void OnArtifactCollected()
    {
        if (!artifactLooted && IsTargetArtifact())
        {
            artifactLooted = true;
            UpdateState();
            FinishQuestStep();
        }
    }

    private bool IsTargetArtifact()
    {
        // Your artifact identification logic here
        return true; // Simplified for example
    }

    private void UpdateState()
    {
        string state = artifactLooted ? "1" : "0";
        string status = artifactLooted ?
            $"{artifactName} recovered" :
            $"Recover {artifactName}";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        try
        {
            artifactLooted = state == "1";
            UpdateState();
        }
        catch
        {
            Debug.LogError("Failed to parse artifact loot state: " + state);
            artifactLooted = false;
            UpdateState();
        }
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.miscEvents.onArtifactCollected -= OnArtifactCollected;
    }
}