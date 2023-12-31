using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using System;

public class CircleWipeTransition : MonoBehaviour
{
    [SerializeField] float transitionDuration = 1f;

    private Canvas canvas;
    private Image blackScreen;

    private Vector2 playerCanvasPos;
    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
    private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        blackScreen = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        blackScreen.material.SetFloat(RADIUS, 1);
        DrawBlackScreen();
    }

    public void LoadSceneTransition(string sceneName, Vector3 positionToLoad)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, positionToLoad));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Vector3 positionToLoad)
    {
        //start the closing transition
        yield return StartCoroutine(Transition(transitionDuration, 1, 0));
        //load scene
        GameManager.Instance.LoadScene(sceneName, positionToLoad);

        // Start the opening transition
        yield return StartCoroutine(Transition(transitionDuration, 0, 1));
    }

    private void DrawBlackScreen()
    {
        //get player position
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
        var playerScreenPos = Camera.main.WorldToScreenPoint(transform.parent.position);


        //draw to image to full screen via canvas rect size
        var canvasRect = canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        //screen conversion from square to actual size
        playerCanvasPos = new Vector2
        {
            x = (playerScreenPos.x / screenWidth) * canvasWidth,
            y = (playerScreenPos.y / screenHeight) * canvasHeight
        };

        var squareValue = 0f;
        if (canvasWidth > canvasHeight)
        {
            //Landscape
            squareValue = canvasWidth;
            playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
        }
        else
        {
            squareValue = canvasHeight;
            playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
        }

        playerCanvasPos /= squareValue;
        var mat = blackScreen.material;
        mat.SetFloat(CENTER_X, playerCanvasPos.x);
        mat.SetFloat(CENTER_Y, playerCanvasPos .y);

        //assign to image
        blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius)
    {
        var time = 0f;
        var mat = blackScreen.material;
        while (time < duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            mat.SetFloat(RADIUS, radius);

            yield return null;
        }
    }
}
