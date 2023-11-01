using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] public float zoomDuration = 0.5f;
    public string InteractionPrompt => _prompt;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJson;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(Interactor interactor)
    {
        DialogueManager.Instance.EnterDialogueMode(inkJson);
        return true;
    }
}
