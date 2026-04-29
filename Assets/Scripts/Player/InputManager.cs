using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    MainController playerControls;
    AnimatorManager animatorManager;
    Animator animator;
    PlayerManager playerManager;
    PlayerCamera playerCamera;

    [Header("Player Movement")]
    public float verticalMovementInput; 
    public float horizontalMovementInput;
    private Vector2 movementInput;

    [Header("Camera Rotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool runInput;
    public bool quickTurnInput;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>(); 
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new MainController();

            playerControls.Movement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Movement.Movement.canceled += i => movementInput = Vector2.zero;
            playerControls.Movement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.Movement.Run.performed += i => runInput = true;
            playerControls.Movement.Run.canceled += i => runInput = false;
            playerControls.Movement.QuickTurn.performed += i => quickTurnInput = true;
            //playerControls.Movement.QuickTurn.canceled += i => quickTurnInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleQuickTurnInput();
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);
    }

    private void HandleCameraInput()
    {
        horizontalCameraInput = cameraInput.x;
        verticalCameraInput = cameraInput.y;

    }


    private void HandleQuickTurnInput()
    {
        if(playerManager.isPerfomingAction)
        {
            return;
        }

        if(quickTurnInput)
        {
            //Play an aimation that turns the player
            quickTurnInput = false;
            animator.SetBool("isPerformingQuickTurn", true);
            animatorManager.PlayAnimationWithoutRootMotion("Run_Hardturn_180", true);
            playerCamera.ApplyQuickTurnCamera();
        }
    }






}
