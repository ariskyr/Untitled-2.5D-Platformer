using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinsQuestStep : QuestStep
{
    private int coinsCollected = 0;
    public int coinsToCollect = 5;

    private void OnEnable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected += CoinCollected;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected -= CoinCollected;
    }

    private void CoinCollected()
    {
        if (coinsCollected < coinsToCollect)
        {
            coinsCollected++;
        }

        if (coinsCollected >= coinsToCollect)
        {
            FinishQuestStep();
        }
    }
}
 