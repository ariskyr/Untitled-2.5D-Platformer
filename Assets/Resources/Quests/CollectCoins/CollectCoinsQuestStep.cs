using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinsQuestStep : QuestStep
{
    public int coinsCollected = 0;
    public int coinsToCollect = 5;


    private void OnEnable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected += CoinCollected;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected -= CoinCollected;
    }

    private void Start()
    {
        UpdateState();
    }


    private void CoinCollected()
    {
        if (coinsCollected < coinsToCollect)
        {
            coinsCollected++;
            UpdateState();
        }

        if (coinsCollected >= coinsToCollect)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = coinsCollected.ToString();
        string status = "Collected " + coinsCollected + " / " + coinsToCollect + " coins.";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        //this is also scary thing to mess up when serializing
        try
        {
            this.coinsCollected = System.Int32.Parse(state);
            UpdateState();
        }
        catch
        {
            Debug.LogError("Failed to parse state string to int: " + state);
        }
    }
}
 