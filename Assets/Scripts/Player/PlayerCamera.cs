using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public InputManager inputManager; 
    public PlayerManager playerManager;

    public Transform cameraPivot;
    public Camera cameraObject;
    public GameObject player;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 cameraRotation;
    private Quaternion targetRotation;

    [Header("Camera speed")]
    public float cameraFollowSmoothTime = 0.2f;
    public float cameraRotationSmoothTime = 0.2f;

    private float lookAmountHorizontal;
    private float lookAmountVertical;
    private float maximumPivotAngle = 15;
    private float minimumPivotAngle = -15;

    private void Awake()
    {
        if (inputManager == null)
            Debug.LogError("PlayerCamera: falta asignar InputManager en el Inspector.");
        if (playerManager == null)
            Debug.LogError("PlayerCamera: falta asignar PlayerManager en el Inspector.");
        if (cameraPivot == null)
            Debug.LogError("PlayerCamera: falta asignar CameraPivot en el Inspector.");
        if (cameraObject == null)
            Debug.LogError("PlayerCamera: falta asignar CameraObject en el Inspector.");
        if (player == null)
            Debug.LogError("PlayerCamera: falta asignar Player en el Inspector.");
    }

    public void HandleAllCameraMovement()
    {
        //Follow the player 
        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraFollowSmoothTime);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
        lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
        lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);

        cameraRotation = Vector3.zero;
        cameraRotation.y = lookAmountVertical;
        targetRotation = Quaternion.Euler(cameraRotation);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotationSmoothTime);
        transform.rotation = targetRotation;

        //If a perfroming a Quick Turn, snap the camera to a 180 position //rollback
        if(inputManager.quickTurnInput)
        {
            inputManager.quickTurnInput = false;
            lookAmountVertical = lookAmountVertical + 180;
            cameraRotation.y = cameraRotation.y + 180;
            transform.rotation = targetRotation;
            //in future, add smooth transition.
        }

        cameraRotation = Vector3.zero;
        cameraRotation.x = lookAmountHorizontal;
        targetRotation = Quaternion.Euler(cameraRotation);
        targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraRotationSmoothTime);
        cameraPivot.localRotation = targetRotation;
    }

}

/*  Scrap code
 *  
 * 
        //if (playerManager.isPerformingQuickTurn && !quickTurnApplied)
        //{
        //    quickTurnApplied = true;
        //    lookAmountVertical -= 180f;
        //}
        //else if (!playerManager.isPerformingQuickTurn)
        //{
        //    quickTurnApplied = false;
        //}
 
 
 */