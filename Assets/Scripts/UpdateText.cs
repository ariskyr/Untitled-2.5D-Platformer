using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour, IDataPersistence
{
    private int buttonPressCount = 0;
    private float elapsedTime = 0;
    [SerializeField] private TextMeshProUGUI buttonPressText;
    [SerializeField] private TextMeshProUGUI timerText;

    public void LoadData(GameData data)
    {
        buttonPressCount = data.buttonPresses;
        elapsedTime = data.timer;
    }

    public void SaveData(GameData data) 
    {
        data.buttonPresses = buttonPressCount;
        data.timer = elapsedTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        TextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.GetTestPressed())
        {
            buttonPressCount++;
            TextUpdate();
        }
        Timer();
    }

    private void TextUpdate()
    {
        buttonPressText.text = "Button presses: " + buttonPressCount;
    }

    private void Timer()
    {
        // the ticking time
        elapsedTime += Time.deltaTime;

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(elapsedTime);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = "Time passed: " + formattedTime;
    }
}
