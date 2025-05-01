using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float transitionDuration = 1.0f;

    public bool IsDisplayed = false;
    
    private IPlayerDirectionProvider _playerDirectionProvider;
    private bool _is3DMode = false;

    private Coroutine currentTransition;
    private bool IsTransitioning = false;
    private bool allowPositionUpdate = true;

    private Vector3 initialPositionRight = new Vector3(-0.55f, 0f, 0f);
    private Vector3 initialPositionLeft = new Vector3(0.55f, 0f, 0f);
    private Vector3 targetPositionRight = new Vector3(-0.356f, 0.142f, 0f);
    private Vector3 targetPositionLeft = new Vector3(0.356f, 0.142f, 0f);

    private void Start()
    {
        _playerDirectionProvider = GetComponentInParent<IPlayerDirectionProvider>();
        if (GetComponentInParent<Player>() != null)
        {
            _is3DMode = true;
            _mainCam = Camera.main;
            if (_mainCam == null && _is3DMode) // Only log error if in 3D mode
            {
                Debug.LogError("InteractionPromptUI: In 3D Mode but no active Camera tagged 'MainCamera'. Billboarding will fail.", this);
            }
        }
        else
        {
            _is3DMode = false; // It's Player2D or something else implementing the interface
        }


        if (_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.LogError("InteractionPromptUI: CanvasGroup component is required. Disabling.", this);
            this.enabled = false;
            return;
        }
        _canvasGroup.alpha = 0; // Initialize alpha
        UpdateIdlePosition();
    }

    private void LateUpdate()
    {
        if (_playerDirectionProvider == null) return;

        if (allowPositionUpdate)
        {
            UpdateIdlePosition();
        }

        // Only perform billboarding if in 3D mode and main camera exists
        if (_is3DMode && _mainCam != null)
        {
            var rotation = _mainCam.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward,
                rotation * Vector3.up);
        }
    }

    private void UpdateIdlePosition()
    {
        bool facingRight = _playerDirectionProvider.facingRight;
        transform.localPosition = facingRight ? initialPositionRight : initialPositionLeft;
    }

    public void SetUp(string promptText)
    {
        if (_playerDirectionProvider == null || _canvasGroup == null) return; // Basic check
        bool facingRight = _playerDirectionProvider.facingRight;

        _promptText.text = promptText;

        Vector3 start = facingRight ? initialPositionRight : initialPositionLeft;
        Vector3 target = facingRight ? targetPositionRight : targetPositionLeft;

        if (IsTransitioning && currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        allowPositionUpdate = false;
        IsTransitioning = true;
        // Use current alpha as start alpha for smooth transition if already partially visible
        currentTransition = StartCoroutine(TransitionCanvasGroupAlpha(_canvasGroup.alpha, 1f, start, target));

        IsDisplayed = true;
    }

    public void Close()
    {
        if (_playerDirectionProvider == null || _canvasGroup == null) return;
        if (!IsDisplayed && !IsTransitioning) return;

        bool facingRight = _playerDirectionProvider.facingRight;

        Vector3 start = transform.localPosition;
        Vector3 target = facingRight ? initialPositionRight : initialPositionLeft;

        if (IsTransitioning && currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        IsTransitioning = true;
        allowPositionUpdate = false;
        // Use current alpha as start alpha
        currentTransition = StartCoroutine(CloseTransition(_canvasGroup.alpha, 0f, start, target));

        IsDisplayed = false;
    }

    private IEnumerator TransitionCanvasGroupAlpha(float startAlpha, float targetAlpha, Vector3 startPos, Vector3 targetPos)
    {
        IsTransitioning = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            if (_canvasGroup == null) yield break;

            float progress = Mathf.Clamp01(elapsedTime / transitionDuration);
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_canvasGroup != null) _canvasGroup.alpha = targetAlpha;
        transform.localPosition = targetPos;
        IsTransitioning = false;
        currentTransition = null;
    }

    private IEnumerator CloseTransition(float startAlpha, float targetAlpha, Vector3 startPos, Vector3 targetPos)
    {
        IsTransitioning = true; // Ensure flag is set

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            // Added null check for safety mid-coroutine
            if (_canvasGroup == null) yield break;

            float progress = Mathf.Clamp01(elapsedTime / transitionDuration);
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_canvasGroup != null) _canvasGroup.alpha = targetAlpha;
        transform.localPosition = targetPos;
        IsTransitioning = false;
        currentTransition = null;
        allowPositionUpdate = true; // Re-enable LateUpdate positioning after closing
    }
}
