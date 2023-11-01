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

    [Header("Ink JSON")]
    [SerializeField] private float _FovOffset = 2f;
    [SerializeField] private Vector3 _rotationOffset = new Vector3(-15f, 0f, 0f);
    [SerializeField] private Vector3 _positionOffset = new Vector3(0, -1f, 0f);

    private int _numFound;
    private bool _hasInteracted = false;
    private readonly Collider[] _colliders = new Collider[3];
    private IInteractable _interactable;
    private PlayerMovement pMovementRef;
    private bool isZoomed = false;
    private Camera _camera;
    private CameraFollow _cameraFollowScript;
    private float _cameraFOV;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraFollowScript = _camera.GetComponent<CameraFollow>();
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
                    if (PlayerMovement.Instance.GetInteractPressed())
                    {
                        if (_interactable is DialogueTrigger)
                        {
                            if (!isZoomed)
                            {
                                isZoomed = true;
                                _cameraFollowScript.canFollow = false;
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
                if (_interactable is DialogueTrigger && isZoomed)
                {
                    _cameraFollowScript.canFollow = true;
                    DialogueManager.Instance.ExitDialogueMode();
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
        float targetFOV = _cameraFOV / _FovOffset;
        float currentFOV = _camera.fieldOfView;
        Vector3 initialCamPos = _camera.transform.position;
        Quaternion initialCamRot = _camera.transform.rotation;

        while (timer <= zoomDuration)
        {
            float t = timer / zoomDuration;

            // This is just for demonstration purposes
            if (isZoomingIn)
            {
                // Zooming in effect
                Quaternion targetRot = Quaternion.Euler(_rotationOffset) * initialCamRot;

                _camera.fieldOfView = Mathf.Lerp(_cameraFOV, targetFOV, t);
                _camera.transform.position = Vector3.Lerp(initialCamPos, initialCamPos + _positionOffset, t);
                _camera.transform.rotation = Quaternion.Lerp(initialCamRot, targetRot, t);
            }
            else
            {
                // Zooming out effect
                Quaternion targetRot = Quaternion.Euler(-_rotationOffset) * initialCamRot;

                _camera.fieldOfView = Mathf.Lerp(currentFOV, _cameraFOV, t);
                _camera.transform.position = Vector3.Lerp(initialCamPos, initialCamPos - _positionOffset, t);
                _camera.transform.rotation = Quaternion.Lerp(initialCamRot, targetRot, t);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
