using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleWipeTransition : MonoBehaviour
{
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] Transform playerTransform;

    private Canvas canvas;
    private Image blackScreen;
    private Material materialInstance;

    private Vector2 playerCanvasPos;
    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
    private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        blackScreen = GetComponentInChildren<Image>();
        materialInstance = new Material(blackScreen.material);
        blackScreen.material = materialInstance;
    }

    private void Start()
    {
        blackScreen.material.SetFloat(RADIUS, 1);
        //DrawBlackScreen();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat(RADIUS, 1);
        }
    }

    public void LoadSceneTransition(string sceneName, Vector3 positionToLoad)
    {
        playerTransform = FindActivePlayerTransform();
        if (playerTransform == null)
        {
            Debug.LogError("CircleWipeTransition: Could not find active Player object tagged 'Player'. Aborting transition. Ensure ONE active player object has the 'Player' tag.");
            return;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName, positionToLoad));
    }

    private Transform FindActivePlayerTransform()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            return playerObject.transform;
        }

        Debug.LogWarning("Could not find GameObject with tag 'Player'. Make sure your active player GameObject ('Player' or 'Player2D') is tagged correctly.");
        return null;

    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Vector3 positionToLoad)
    {
        if (materialInstance == null || playerTransform == null)
        {
            Debug.LogError("Transition prerequisites not met (Material or Initial Player Transform).");
            yield break;
        }

        CalculateAndSetCenter(playerTransform.position);
        //start the closing transition
        yield return StartCoroutine(Transition(transitionDuration, 1, 0));
        //load scene
        GameManager.Instance.LoadScene(sceneName, positionToLoad);

        //wait a frame
        yield return null;

        // Start the opening transition
        playerTransform = FindActivePlayerTransform();

        if (playerTransform != null)
        {
            // Calculate center based on potentially new player position/camera
            CalculateAndSetCenter(playerTransform.position);
        }
        else
        {
            Debug.LogWarning("Player not found after scene load. Wipe may open from old center or default location.");
            CalculateAndSetCenter(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane + 1f)));
        }

        // 5. Start opening transition
        yield return StartCoroutine(Transition(transitionDuration, 0, 1));
    }

    private void CalculateAndSetCenter(Vector3 worldPosition)
    {
        if (materialInstance == null || Camera.main == null) return;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);
        materialInstance.SetFloat(CENTER_X, viewportPoint.x);
        materialInstance.SetFloat(CENTER_Y, viewportPoint.y);
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
        if (materialInstance == null) yield break;
        var time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float radius = Mathf.Lerp(beginRadius, endRadius, t);
            materialInstance.SetFloat(RADIUS, radius);
            yield return null;
        }
        materialInstance.SetFloat(RADIUS, endRadius);
    }
}
