using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscEvents
{
    public event Action onCoinCollected;
    public void CoinCollected()
    {
        if (onCoinCollected != null)
        {
            onCoinCollected();
        }
    }

    public event Action<string> onKnightKilled;
    public void KnightKilled(string knightGUID)
    {
        if (onKnightKilled != null)
        {
            onKnightKilled(knightGUID);
        }
    }

    public event Action onArtifactCollected;
    public void ArtifactCollected()
    {
        if (onArtifactCollected != null)
        {
            onArtifactCollected();
        }
    }
}
