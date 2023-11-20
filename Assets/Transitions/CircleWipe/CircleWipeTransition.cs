using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;


public class CircleWipeTransition : MonoBehaviour
{
    [SerializeField] public float transitionTime = 1f;
    [SerializeField] public Image _maskTransition;

    private bool _isStarting = false;
    private RectTransform _canvas;
    float _screen_h = 0;
    float _screen_w = 0;
    float _radius = 1;
    float _counter;

    // Start is called before the first frame update
    void Start()
    {

        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        GetCharacterPosition();
    }

    // Update is called once per frame
    void Update()
    {
        _counter += Time.deltaTime;

        if (_counter >= 0.5)
        {
            if (_isStarting)
            {
                if (_radius > 0)
                {
                    _radius -= Time.deltaTime / transitionTime;
                    _maskTransition.material.SetFloat("Radius", _radius);
                }
            }
            else
            {
                if (_radius < 1)
                {
                    _radius += Time.deltaTime / transitionTime;
                    _maskTransition.material.SetFloat("Radius", _radius);
                }
            }
        }
    }

    void GetCharacterPosition()
    {
        Vector3 targetPosition = gameObject.transform.position;
        //targetPosition.y -= 0.5f; // small adjustment
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPosition);

        float characterScreen_w = 0;
        float characterScreen_h = 0;

        if(_isStarting)
        {
            _radius = 0;
            _maskTransition.material.SetFloat("Radius", _radius);
        }
        else
        {
            _radius = 1;
            _maskTransition.material.SetFloat("Radius", _radius);
        }

        _canvas = GetComponent<RectTransform>();
        _screen_h = Screen.height;
        _screen_w = Screen.width;

        if(_screen_w < _screen_h)
        {
            _maskTransition.rectTransform.sizeDelta = new Vector2(_canvas.rect.height, _canvas.rect.height);
            float newScreenPos_x = screenPos.x + (_screen_h - _screen_w) / 2;

            characterScreen_w = (newScreenPos_x * 100) / _screen_h;
            characterScreen_w /= 100;

            characterScreen_h = (screenPos.y * 100) / _screen_h;
            characterScreen_h /= 100;
        }
        else
        {
            _maskTransition.rectTransform.sizeDelta = new Vector2(_canvas.rect.width, _canvas.rect.width);
            float newScreenPos_y = screenPos.y + (_screen_w - _screen_h) / 2;

            characterScreen_w = (screenPos.x * 100) / _screen_w;
            characterScreen_w /= 100;

            characterScreen_h = (newScreenPos_y * 100) / _screen_w;
            characterScreen_h /= 100;
        }

        _maskTransition.material.SetFloat("Center_X", characterScreen_w);
        _maskTransition.material.SetFloat("Center_Y", characterScreen_h);
    }

    public void StartTransition(string levelToLoad, Vector3 positionToLoad)
    {
        StartCoroutine(LoadLevelCoroutine(levelToLoad, positionToLoad));
    }

    IEnumerator LoadLevelCoroutine(string sceneName, Vector3 positionToLoad)
    {
        _isStarting = true; // Set the loading flag to true
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        transform.parent.position = positionToLoad;
        _isStarting = false;
    }
}
