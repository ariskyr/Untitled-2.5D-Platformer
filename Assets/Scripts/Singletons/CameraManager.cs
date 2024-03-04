using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Pixelplacement;

public class CameraManager : GenericSingleton<CameraManager>, IDataPersistence
{
    [Header("Camera Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;

    public bool zoomingIn = false;
    public bool canFollow = true;

    private Camera playerCamera;
    private float defaultCameraFOV;
    private Vector3 defaultCameraPos;
    private Quaternion defaultCameraRot;
    private Vector3 offset;

    private Coroutine zoomCoroutine;

    protected override void Awake()
    {
        base.Awake();
        playerCamera = Camera.main;
        defaultCameraFOV = playerCamera.fieldOfView;
        //set the default pos and rot before zoom in
        defaultCameraPos = transform.position;
        defaultCameraRot = transform.rotation;
    }

    public void LoadData(GameData data)
    {
        //Default positions for the camera per scene
        // per scene position save (? maybe)
        transform.position = data.cameraPosition;
    }

    public void SaveData(GameData data)
    {
        //Default positions for the camera per scene
        // per scene position save (? maybe)
        data.cameraPosition = transform.position;
    }

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (canFollow)
        {
            FollowTarget(target, smoothing);
        }
    }

    //Follow a specific target object
    public void FollowTarget(Transform target, float smoothing)
    {
        Vector3 targetCamPos = target.position + offset;

        //Check distance between cam pos and player pos
        float distance = Vector3.Distance(transform.position, targetCamPos);
        if (distance > 10f)
        {
            transform.position = targetCamPos;
        }

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void CameraZoomIn(float zoomDuration, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        StopZoomCoroutine();
        //set the default pos and rot before zoom in
        defaultCameraPos = transform.position;
        defaultCameraRot = transform.rotation;
        zoomCoroutine = StartCoroutine(CameraZoom(true, zoomDuration, positionOffset, rotationOffset, fovOffset));
    }

    public void CameraZoomOut(float zoomDuration, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        StopZoomCoroutine();

        zoomCoroutine = StartCoroutine(CameraZoom(false, zoomDuration, positionOffset, rotationOffset, fovOffset));
    }


    public void CameraShake(float strength, float duration = 1.0f)
    {
        Tween.Shake(transform, transform.localPosition, new Vector3(strength, strength, 0), duration, 0);
    }

    public void StopZoomCoroutine()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }
    }

    private IEnumerator CameraZoom(bool isZooming, float zoomDuration, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        zoomingIn = true;
        float timer = 0;
        float targetFOV = isZooming ? defaultCameraFOV / fovOffset : defaultCameraFOV;
        float currentFOV = playerCamera.fieldOfView;
        Vector3 currentCamPos = playerCamera.transform.position;
        Quaternion currentCamRot = playerCamera.transform.rotation;

        while (timer <= zoomDuration)
        {
            float t = timer / zoomDuration;

            // Zooming in effect
            if (isZooming)
            {
                Quaternion targetRot = Quaternion.Euler(rotationOffset) * currentCamRot;

                playerCamera.fieldOfView = Mathf.Lerp(defaultCameraFOV, targetFOV, t);
                playerCamera.transform.SetPositionAndRotation(Vector3.Lerp(currentCamPos, currentCamPos + positionOffset, t),
                                                         Quaternion.Lerp(currentCamRot, targetRot, t));
            }
            // Zooming out effect
            else
            {

                playerCamera.fieldOfView = Mathf.Lerp(currentFOV, defaultCameraFOV, t);
                playerCamera.transform.SetPositionAndRotation(Vector3.Lerp(currentCamPos, target.position + offset, t),
                                                         Quaternion.Lerp(currentCamRot, defaultCameraRot, t));
            }
            timer += Time.deltaTime;
            yield return null;
        }
        zoomingIn = false;
    }

}
