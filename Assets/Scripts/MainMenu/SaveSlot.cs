using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile ID")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI slotNameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI buttonPressesText;

    private Button saveSlotButton;
    private bool dataExistence = false;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        slotNameText.text = profileID;

        if (data == null)
        {
            //theres no data for this slot
            dataExistence = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            //there is data for this slot
            dataExistence = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            //take the time passed and format it
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(data.timer);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            timerText.text = formattedTime;
            buttonPressesText.text = "Button presses: " + data.buttonPresses;
        }
    }

    public string GetProfileID()
    {
        return this.profileID;
    }

    public bool GetDataExistence()
    {
        return this.dataExistence;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
