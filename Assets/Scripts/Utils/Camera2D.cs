using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    private void Start()
    {
        Player2D player = Player2D.Instance;
        if (player == null)
        {
            Debug.LogError("Player2D instance not found.");
            return;
        }
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
    }
}
