using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    MainController playerControls;
    AnimatorManager animatorManager;

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


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>(); 

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






}
