using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerController[] players;
    [SerializeField] int currentPlayerIndex = 0;

    private Camera mainCamera;
    private PlayerController currentSub;

    private void SwapPlayer()
    {
        //just move through array normally
        currentPlayerIndex = (currentPlayerIndex + 1 + players.Length) % players.Length;
        currentSub = players[currentPlayerIndex];
        CameraSetup();
    }

    public void CameraSetup()
    {
        mainCamera.transform.parent = currentSub.cameraPosition;
        mainCamera.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        currentSub = players[currentPlayerIndex];
        CameraSetup();
    }

    private void Update()
    {
        //check for sub switch here
        if(Input.GetKeyUp(KeyCode.E))
        {
            SwapPlayer();
        }
        else
        {
            currentSub.RunUpdate();
        }
    }

    private void FixedUpdate()
    {
        currentSub.RunFixedUpdate();
    }

    private void LateUpdate()
    {
        currentSub.RunLateUpdate();
    }
}
