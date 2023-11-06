using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    private Story currentStory;
    public bool DialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine = false;

    public static DialogueManager Instance { get; private set; }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Coroutine displayLineCoroutine;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than 1 player movement was found");
        }
        Instance = this;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        //get all choices
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

    }

    private void Update()
    {
        if (!DialogueIsPlaying)
        {
            return;
        }
        if (canContinueToNextLine &&
            currentStory.currentChoices.Count == 0 &&
            PlayerMovement.Instance.GetInteractPressed())
        {
            ContinueStory();
        }
    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //reset tag defaults
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();
    }
    public void ExitDialogueMode()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            //handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can supporrt. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        //choice box init that are available for this line of dialogue
        foreach(Choice choice in currentChoices) 
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //hide the rest
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appopriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event system rrequires we clear it first then wait for 1 frame and set it
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    private IEnumerator DisplayLine(string line)
    {
        canContinueToNextLine = false;
        continueIcon.SetActive(false); // hide while typing
        HideChoices();

        int visibleCharacters = 0;

        dialogueText.text = line; //init
        dialogueText.maxVisibleCharacters = visibleCharacters; //hide all chars
        while (visibleCharacters < line.Length)
        {
            if (visibleCharacters > 3 && PlayerMovement.Instance.GetInteractPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            visibleCharacters++;
            dialogueText.maxVisibleCharacters = visibleCharacters;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.SetActive(true); // show again
        DisplayChoices(); //display choices if any
        canContinueToNextLine = true;
    }

}
