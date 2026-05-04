using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public PlayerInventoryManager playerInventoryManager;
    public InputManager inputManager;
    public PlayerSanityManager playerSanityManager;
    public PlayerUIManager playerUI;

    Animator animator;
    PlayerLocomotionManager playerLocomotionManager;

    [Header("Player Flags")]
    public bool isPerformingAction;
    public bool isPerformingQuickTurn;
    public bool canInteract;

    private void Awake()
    {
        //playerCamera = FindObjectOfType<PlayerCamera>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerSanityManager = GetComponent<PlayerSanityManager>();
        playerUI = FindObjectOfType<PlayerUIManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        isPerformingAction = animator.GetBool("isPerformingAction");
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
