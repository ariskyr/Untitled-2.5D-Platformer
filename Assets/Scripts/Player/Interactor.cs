using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    private int _numFound;
    private bool _hasInteracted = false;
    private readonly Collider[] _colliders = new Collider[3];
    private IInteractable _interactable;

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, 
                                                _colliders, (int)_interactableMask);

        if (_numFound > 0 )
        {
            if (_colliders[0].TryGetComponent(out _interactable)) 
            {
                //if hasn't interacted
                if (!_hasInteracted)
                {
                    // show the interact UI if its not displayed
                    if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);


                    // on button press, interact
                    if (InputManager.Instance.GetInteractPressed())
                    {
                        _hasInteracted = true;
                        _interactionPromptUI.Close();
                        _interactable.Interact(this);
                    }
                }
                else
                {
                    // if dialogue is not playing
                    if (_interactable is DialogueTrigger dialogueTrigger && !DialogueManager.Instance.DialogueIsPlaying)
                    {
                        _hasInteracted = false;
                        CameraManager.Instance.canFollow = true;
                        CameraManager.Instance.CameraZoomOut(dialogueTrigger.zoomDuration, dialogueTrigger.positionOffset, 
                            dialogueTrigger.rotationOffset, dialogueTrigger.fovOffset);
                    }
                }
            }
        }
        else
        {
            // set interactable to null and close UI
            _hasInteracted = false;
            if (_interactable != null)
            {
                if (_interactable is DialogueTrigger dialogueTrigger)
                {
                    CameraManager.Instance.canFollow = true;
                    DialogueManager.Instance.ExitDialogueMode();
                    CameraManager.Instance.CameraZoomOut(dialogueTrigger.zoomDuration, dialogueTrigger.positionOffset,
                        dialogueTrigger.rotationOffset, dialogueTrigger.fovOffset);
                }
                _interactable = null;
            }
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
