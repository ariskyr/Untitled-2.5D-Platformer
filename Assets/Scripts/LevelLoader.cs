using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject mainObject;
    [SerializeField] public Animator transition;
    [SerializeField] public float transitionTime = 1f;

    private void Start()
    {
        //Set Crossfade to inactive
        mainObject.SetActive(false);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        mainObject.SetActive(true);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
