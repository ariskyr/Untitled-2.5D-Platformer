using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float transitionDuration = 1.0f;

    public bool IsDisplayed = false;
    private PlayerController _playerController;
    private Coroutine currentTransition;
    private bool IsTransitioning = false;
    private bool allowPositionUpdate = true;
    private Vector3 initialPositionRight = new Vector3(-0.55f, 0f, 0f);
    private Vector3 initialPositionLeft = new Vector3(0.55f, 0f, 0f);
    private Vector3 targetPositionRight = new Vector3(-0.356f, 0.142f, 0f);
    private Vector3 targetPositionLeft = new Vector3(0.356f, 0.142f, 0f);
    private bool playerDirection = false;

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();

        _mainCam = Camera.main;
        _canvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        if (allowPositionUpdate)
        {
            playerDirection = _playerController.m_Directionright;
            if (playerDirection)
            {
                gameObject.transform.localPosition = initialPositionRight;
            }
            else
            {
                gameObject.transform.localPosition = initialPositionLeft;
            }
        }
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, 
            rotation * Vector3.up);
    }

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;

        Vector3 start = playerDirection ? initialPositionRight : initialPositionLeft;
        Vector3 target = playerDirection ? targetPositionRight : targetPositionLeft;

        if (IsTransitioning)
        {
            StopCoroutine(currentTransition);
            IsTransitioning = false;
        }
        allowPositionUpdate = false;
        currentTransition = StartCoroutine(TransitionCanvasGroupAlpha(0f, 1f, start, target));

        IsDisplayed = true;
    }

    public void Close()
    {
        Vector3 start = gameObject.transform.localPosition;
        Vector3 oppositeDirection = !playerDirection ? initialPositionLeft : initialPositionRight;

        if (IsTransitioning)
        {
            StopCoroutine(currentTransition);
            IsTransitioning = false;
        }
        StartCoroutine(CloseTransition(1f, 0f, start, oppositeDirection));

        IsDisplayed =false;
    }

    private IEnumerator TransitionCanvasGroupAlpha(float startAlpha, float targetAlpha, Vector3 start, Vector3 target)
    {
        IsTransitioning = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);
            gameObject.transform.localPosition = Vector3.Lerp(start, target, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
        IsTransitioning = false;
    }

    private IEnumerator CloseTransition(float startAlpha, float targetAlpha, Vector3 start, Vector3 target)
    {
        IsTransitioning = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);
            gameObject.transform.localPosition = Vector3.Lerp(start, target, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
        IsTransitioning = false;
        allowPositionUpdate = true; // Enable LateUpdate after transition
    }
}
