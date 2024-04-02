using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class VisitBoxesQuestStep : QuestStep
{
    [Header("Config")]
    [SerializeField] private string boxNumberString = "first";

    private void Start()
    {
        string status = "Visit the " + boxNumberString + " box";
        ChangeState("", status);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string status = "Visited the " + boxNumberString + " box";
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // no state is needed for this quest step
    }
}
