using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    InputManager inputManager;
    PlayerLocomotionManager playerLocomotionManager;


    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotionManager.HandleMovementPublic();
        playerLocomotionManager.HandleRotationPublic();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }


}
