using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueManager : GenericSingleton<DialogueManager>
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    [SerializeField] private bool makePredictable;

    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    private AudioSource audioSource;
    private Story currentStory;
    public bool DialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine = false;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Coroutine displayLineCoroutine;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";

    private DialogueVariables dialogueVariables;

    protected override void Awake()
    {
        //instantiate from base class
        base.Awake();
        //the rest of the instantiation
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        audioSource = gameObject.AddComponent<AudioSource>();
        currentAudioInfo = defaultAudioInfo;
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
        InitializeAudioInfoDictionary();
    }


    private void InitializeAudioInfoDictionary()
    {
        //initialize it with the default
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>
        {
            { defaultAudioInfo.id, defaultAudioInfo }
        };
        //add the rest
        foreach (DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        audioInfoDictionary.TryGetValue(id, out DialogueAudioInfoSO audioInfo);
        if (audioInfo != null)
        {
            currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
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
            InputManager.Instance.GetInteractPressed())
        {
            ContinueStory();
        }
    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //var listener
        dialogueVariables.StartListening(currentStory);

        //reset tag defaults
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();
    }
    public void ExitDialogueMode()
    {
        //clear the coroutine if playing
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        //var listener
        if (currentStory != null)
        {
            dialogueVariables.StopListening(currentStory);
        }

        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        //go back to default audio config
        SetCurrentAudioInfo(defaultAudioInfo.id);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            //handle tags
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));

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
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        //set variables
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        if (currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }

            AudioClip soundClip;
            if (makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];
                //pitch
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                if (pitchRangeInt != 0)
                {
                    int predicateblePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predicateblePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                //randomisation of sound
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }

            audioSource.PlayOneShot(soundClip);
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

        bool isAddingRichTextTag = false;

        int visibleCharacters = 0;
        dialogueText.text = line; //init
        dialogueText.maxVisibleCharacters = 0; //hide all chars
        foreach (char letter in line.ToCharArray())
        {
            //skip
            if (dialogueText.maxVisibleCharacters > 3 && InputManager.Instance.GetInteractPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.maxVisibleCharacters++;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                //play sound
                PlayDialogueSound(visibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                //increment
                visibleCharacters++;
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true); // show again
        DisplayChoices(); //display choices if any
        canContinueToNextLine = true;
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        dialogueVariables.Variables.TryGetValue(variableName, out Ink.Runtime.Object variableValue);
        if (variableValue == null)
        {
            Debug.Log("Ink variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    public void OnApplicationQuit()
    {
        dialogueVariables?.SaveVariables();
    }
}
