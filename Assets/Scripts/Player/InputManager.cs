using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    MainController playerControls;
    AnimatorManager animatorManager;
    Animator animator;
    PlayerManager playerManager;

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
    public bool interactInput;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>(); 
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
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
            playerControls.Player_Actions.Interact.performed += i => interactInput = true;
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
        HandleInteractionInput();
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

        if(quickTurnInput) //Rollback
        {
            //Play an aimation that turns the player
            animator.SetBool("isPerformingQuickTurn", true);
            animatorManager.PlayAnimationWithoutRootMotion("Run_Hardturn_180", true);
        }
    }

    private void HandleInteractionInput()
    {
        if(interactInput)
        {
            if(!playerManager.canInteract)
            {
                interactInput = false;
            }
        }
        else
        {
            interactInput = false;
        }
    }




}
