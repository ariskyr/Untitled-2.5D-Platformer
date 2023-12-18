using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [Header("Camera Zoom")]
    public float zoomDuration = 0.5f;
    public Vector3 positionOffset = new(0, -1f, 0f);
    public Vector3 rotationOffset = new(-15f, 0f, 0f);
    public float fovOffset = 2;
    public string InteractionPrompt => _prompt;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJson;
    public bool Interact(Interactor interactor)
    {
        //zoom in camera
        CameraManager.Instance.canFollow = false;
        CameraManager.Instance.CameraZoomIn(zoomDuration, positionOffset, rotationOffset, fovOffset);
        //enter dialogue
        DialogueManager.Instance.EnterDialogueMode(inkJson);
        return true;
    }
}
