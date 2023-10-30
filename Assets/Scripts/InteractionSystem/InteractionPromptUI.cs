using System.Collections;
using System.Collections.Generic;
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
    private Coroutine currentTransition;
    private bool IsTransitioning = false;

    private void Start()
    {
        _mainCam = Camera.main;
        _canvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, 
            rotation * Vector3.up);
    }

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        
        if (IsTransitioning)
        {
            StopCoroutine(currentTransition);
            IsTransitioning = false;
        }
        currentTransition = StartCoroutine(TransitionCanvasGroupAlpha(0f, 1f));

        IsDisplayed = true;
    }

    public void Close()
    {
        if (IsTransitioning)
        {
            StopCoroutine(currentTransition);
            IsTransitioning = false;
        }
        currentTransition = StartCoroutine(TransitionCanvasGroupAlpha(1f, 0f));

        IsDisplayed =false;
    }

    private IEnumerator TransitionCanvasGroupAlpha(float startAlpha, float targetAlpha)
    {
        IsTransitioning = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
        IsTransitioning = false;
    }
}
