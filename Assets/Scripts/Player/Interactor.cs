using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    private IInteractable _currentInteractable;
    private bool _hasInteracted = false;
    private readonly Collider[] _colliders3D = new Collider[3];
    private readonly Collider2D[] _colliders2D = new Collider2D[3];

    private bool _is3DMode;

    private void Awake()
    {
        // Determine which player
        _is3DMode = (GetComponentInParent<Player>() != null);

        if (_interactionPoint == null)
            Debug.LogError("Interaction Point transform is not assigned!", this);
        if (_interactionPromptUI == null)
            Debug.LogError("Interaction Prompt UI is not assigned!", this);
    }

    private void Update()
    {
        if (_interactionPoint == null || _interactionPromptUI == null) return;

        int numFound = 0;
        IInteractable foundInteractableThisFrame = null;
        if (_is3DMode)
        {
            numFound = Physics.OverlapSphereNonAlloc(
                _interactionPoint.position,
                _interactionPointRadius,
                _colliders3D,
                _interactableMask);

            if (numFound > 0)
            {
                _colliders3D[0].TryGetComponent(out foundInteractableThisFrame);
            }
        }
        else // 2D Mode
        {
            numFound = Physics2D.OverlapCircleNonAlloc(
                _interactionPoint.position,
                _interactionPointRadius,
                _colliders2D,
                _interactableMask);

            if (numFound > 0)
            {
                _colliders2D[0].TryGetComponent(out foundInteractableThisFrame);
            }
        }

        // Case 1: An interactable is detected this frame
        if (foundInteractableThisFrame != null)
        {
            // If it's a NEW interactable entering range (or switching focus)
            if (foundInteractableThisFrame != _currentInteractable)
            {
                // If we were focused on something else, close its prompt first
                if (_currentInteractable != null && _interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.Close();
                }
                // Update focus to the new interactable
                _currentInteractable = foundInteractableThisFrame;
                _hasInteracted = false; // Reset interaction state for the new target
            }

            // Now, handle logic for the _currentInteractable
            if (!_hasInteracted)
            {
                // Show prompt if not already displayed
                if (!_interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.SetUp(_currentInteractable.InteractionPrompt);
                }

                // Check for interaction input
                if (InputManager.Instance.GetInteractPressed())
                {
                    _hasInteracted = true;
                    _interactionPromptUI.Close();
                    _currentInteractable.Interact(this); // Perform the interaction
                }
            }
            else // Already interacted, handle post-interaction (like DialogueTrigger check)
            {
                if (_currentInteractable is DialogueTrigger dialogueTrigger && !DialogueManager.Instance.DialogueIsPlaying)
                {
                    _hasInteracted = false;
                    CameraManager.Instance.canFollow = true;
                    CameraManager.Instance.CameraZoomOut(dialogueTrigger.zoomDuration, dialogueTrigger.positionOffset,
                        dialogueTrigger.rotationOffset, dialogueTrigger.fovOffset);
                }
            }
        }
        // Case 2: NO interactable component was found on detected colliders OR no colliders detected
        else
        {
            // If we *were* focused on an interactable last frame, but not anymore...
            if (_currentInteractable != null)
            {
                if (_interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.Close();
                }

                if (_currentInteractable is DialogueTrigger dialogueTrigger)
                {
                    DialogueManager.Instance.ExitDialogueMode();
                    CameraManager.Instance.canFollow = true;
                    CameraManager.Instance.CameraZoomOut(dialogueTrigger.zoomDuration, dialogueTrigger.positionOffset,
                        dialogueTrigger.rotationOffset, dialogueTrigger.fovOffset);
                }

                // Clear focus and interaction state
                _currentInteractable = null;
                _hasInteracted = false;
            }
            // If _currentInteractable was already null, do nothing.
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
