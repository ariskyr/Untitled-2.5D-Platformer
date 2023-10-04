using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private TextMeshProUGUI _promptText;

    public bool IsDisplayed = false;

    private void Start()
    {
        _mainCam = Camera.main;
        _UIPanel.SetActive(false);
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
        _UIPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        _UIPanel?.SetActive(false);
        IsDisplayed =false;
    }
}
