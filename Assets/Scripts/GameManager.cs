using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IMPORTANT: MAKE SURE GAME MANAGER EXISTS IN THE FIRST SCENE THAT WILL BE LOADED ON GAME START
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private string[] scenesWithCrossfade;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
