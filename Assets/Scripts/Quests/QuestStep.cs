using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            
            //TODO: advance to next step
            Destroy(gameObject);
        }
    }

}
