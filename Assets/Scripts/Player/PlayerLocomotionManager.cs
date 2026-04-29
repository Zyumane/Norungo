using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    protected InputManager inputManager;
    protected PlayerManager playerManager;
    
    public  Rigidbody playerRigidbody;

    [Header("Camera Transform")]
    public Transform playerCamera;

    [Header("Movement Speed")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 3.5f;
    public float quickTurnSpeed = 8;

    [Header("Rotation Variables")]
    protected Quaternion targetRotation;
    protected Quaternion playerRotation;

    [Header("Movement Variables")]
    protected Vector3 moveDirection;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllLocomotion()
    {
        HandleRotation();
        HandleMovement();
    }

    public void HandleRotationPublic() => HandleRotation();
    public void HandleMovementPublic() => HandleMovement();

    private void HandleRotation()
    {
        Vector3 targetDir = Camera.main.transform.forward * inputManager.verticalMovementInput;
        targetDir += Camera.main.transform.right * inputManager.horizontalMovementInput;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        playerRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.fixedDeltaTime);
        playerRigidbody.MoveRotation(playerRotation);

        if (playerManager.isPerformingQuickTurn)
        {
            targetRotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, quickTurnSpeed * Time.fixedDeltaTime);
            playerRigidbody.MoveRotation(playerRotation);
        }
    }

    private void HandleMovement()
    {
        moveDirection = Camera.main.transform.forward * inputManager.verticalMovementInput;
        moveDirection += Camera.main.transform.right * inputManager.horizontalMovementInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (inputManager.runInput)
            moveDirection *= runSpeed;
        else
            moveDirection *= walkSpeed;

        //transform.position += moveDirection * Time.deltaTime;
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
        playerRigidbody.velocity = projectedVelocity;
    }
}
