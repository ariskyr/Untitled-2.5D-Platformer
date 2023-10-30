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
    [SerializeField] private float zoomDuration = 0.5f;

    private int _numFound;
    private bool _hasInteracted = false;
    private readonly Collider[] _colliders = new Collider[3];
    private IInteractable _interactable;
    private PlayerMovement pMovementRef;
    private bool isZoomed = false;
    private Camera _camera;
    private float _cameraFOV;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraFOV = _camera.fieldOfView;
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, 
                                                _colliders, (int)_interactableMask);

        if (_numFound > 0 )
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null) 
            {
                //if hasn't interacted
                if (!_hasInteracted)
                {
                    // show the interact UI if its not displayed
                    if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                    // on button press, interact
                    pMovementRef = FindObjectOfType<PlayerMovement>();
                    if (pMovementRef.GetInteractPressed())
                    {
                        if (_interactable is DialogueTrigger dialogueTrigger)
                        {
                            if (!isZoomed)
                            {
                                isZoomed = true;
                                StartCoroutine(CameraZoom(isZoomingIn: true));
                            }
                        }
                        _hasInteracted = true;
                        _interactionPromptUI.Close();
                        _interactable.Interact(this);
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
                if (_interactable is DialogueTrigger dialogueTrigger && isZoomed)
                {
                    StartCoroutine(CameraZoom(isZoomingIn: false));
                    isZoomed = false;
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

    private IEnumerator CameraZoom(bool isZoomingIn)
    {
        float timer = 0;
        float targetFOV = _cameraFOV / 2;
        float currentFOV = _camera.fieldOfView;

        while (timer <= zoomDuration)
        {
            float t = timer / zoomDuration;

            // This is just for demonstration purposes
            if (isZoomingIn)
            {
                _camera.fieldOfView = Mathf.Lerp(_cameraFOV, targetFOV, t); // Zooming in effect
            }
            else
            {
                _camera.fieldOfView = Mathf.Lerp(currentFOV, _cameraFOV, t); // Zooming out effect
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
