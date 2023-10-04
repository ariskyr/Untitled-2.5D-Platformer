using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{  
    [SerializeField] private string _prompt;
    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float timeToOpen = 1f;
    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0;

    [Header("Transition to Level")]
    [SerializeField] private CircleWipeTransition transition;
    [SerializeField] private string levelToLoad;

    public string InteractionPrompt => _prompt;
    public bool IsOpen = false;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        //Forward signifies where the door is oriented
        Forward = transform.forward;
    }

    //Interact with the object
    public bool Interact(Interactor interactor)
    {
        //start coroutine to open door
        bool isOpened = Open(interactor.transform.position);
        //load the next level
        transition.StartTransition(levelToLoad);
        return isOpened;
    }

    public bool Open(Vector3 UserPosition)
    {
        if (!IsOpen) 
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (IsRotatingDoor) 
            {
                float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
                return true;
            }
        }
        return false;
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while(time < timeToOpen)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private  IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while (time < timeToOpen)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation , time);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
