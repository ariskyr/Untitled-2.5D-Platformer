using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Collect Artifact";

    public bool Interact(Interactor interactor)
    {
        GameEventsManager.Instance.miscEvents.ArtifactCollected();
        Destroy(gameObject);
        return true;
    }
}
