using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerManager playerManager;
    private Transform cameraTransform;

    public Rigidbody playerRigidbody;

    [Header("Camera Transform")]
    public Transform playerCamera;

    [Header("Movement Speed")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 3.5f;
    public float quickTurnSpeed = 8;

    [Header("Rotation Variables")]
    private Quaternion targetRotation;
    private Quaternion playerRotation;

    [Header("Movement Variables")]
    private Vector3 moveDirection;

    private bool quickTurnTargetCaptured = false;
    private Quaternion quickTurnTargetRot;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        cameraTransform = Camera.main.transform;
    }

    //public void HandleAllLocomotion()
    //{
    //    HandleRotation();
    //    HandleMovement();
    //}

    public void HandleRotationPublic() => HandleRotation();
    public void HandleMovementPublic() => HandleMovement();

    private void HandleRotation()
    {
        // Quick Turn tiene prioridad — rota 180° y sale
        if (playerManager.isPerformingQuickTurn)
        {
            if (!quickTurnTargetCaptured)
            {
                quickTurnTargetRot = transform.rotation * Quaternion.Euler(0, 180f, 0);
                quickTurnTargetCaptured = true;
            }
            playerRotation = Quaternion.Slerp(transform.rotation, quickTurnTargetRot, quickTurnSpeed * Time.deltaTime);
            playerRigidbody.MoveRotation(playerRotation);
            return;
        }
        else
        {
            quickTurnTargetCaptured = false;
        }

        // El personaje sigue el forward de la cámara proyectado en XZ
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (cameraForward == Vector3.zero)
            cameraForward = transform.forward;

        bool isMoving = inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0;
        
        float currentRotSpeed;

        if (isMoving)
        {
            currentRotSpeed = rotationSpeed;
        }
        else
        {
            currentRotSpeed = (float)(rotationSpeed * 3f);
        }

        Quaternion targetRot = Quaternion.LookRotation(cameraForward);
        playerRotation = Quaternion.Slerp(transform.rotation, targetRot, currentRotSpeed * Time.fixedDeltaTime);
        playerRigidbody.MoveRotation(playerRotation);
    }

    private void HandleMovement()
    {
        moveDirection = cameraTransform.forward * inputManager.verticalMovementInput;
        moveDirection += cameraTransform.right * inputManager.horizontalMovementInput;
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
/*Scrap code - Manual RollBack
    
        Quaternion targetRot = Quaternion.LookRotation(cameraForward);
        playerRotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        playerRigidbody.MoveRotation(playerRotation);

        
       Vector3 targetDir = cameraTransform.forward * inputManager.verticalMovementInput;
       targetDir += cameraTransform.right * inputManager.horizontalMovementInput;
       targetDir.Normalize();
       targetDir.y = 0;

       if (targetDir == Vector3.zero)
           targetDir = transform.forward;

       Quaternion tr = Quaternion.LookRotation(targetDir);
       playerRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.fixedDeltaTime);
       playerRigidbody.MoveRotation(playerRotation);

       //video manual insert code
       if (inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0)
           transform.rotation = playerRotation;

       if (playerManager.isPerformingQuickTurn) //Rollback
       {
           targetRotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
           playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, quickTurnSpeed * Time.deltaTime);
       }

 */