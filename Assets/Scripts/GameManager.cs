using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IMPORTANT: MAKE SURE GAME MANAGER EXISTS IN THE FIRST SCENE THAT WILL BE LOADED ON GAME START
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string[] scenesWithCrossfade;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
