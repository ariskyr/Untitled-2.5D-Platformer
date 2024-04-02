using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerEnterHandler
{
    [Header("Profile ID")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private SaveSlotsUI slotsUI;
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI slotNameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI sceneText;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

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
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            //there is data for this slot
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            //take the time passed and format it
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(data.timer);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            timerText.text = formattedTime;
            sceneText.text = "Scene: " + data.lastScene;
        }
    }

    public string GetProfileID()
    {
        return this.profileID;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }

    //this is called everytime a save slot is hovered
    public void OnPointerEnter(PointerEventData eventData)
    {
        //disable hover if button is not interactable
        if (!saveSlotButton.interactable) return;

        //get hovered id if button is hovered
        slotsUI.currentlyHoveredProfileId = this.profileID;
    }
}
