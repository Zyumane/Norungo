using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    InputManager inputManager;
    Animator animator;
    PlayerLocomotionManager playerLocomotionManager;

    [Header("Player Flags")]
    public bool isPerfomingAction;
    public bool isPerformingQuickTurn;


    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        isPerfomingAction = animator.GetBool("isPerformingAction");
        isPerformingQuickTurn = animator.GetBool("isPerformingQuickTurn");
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
