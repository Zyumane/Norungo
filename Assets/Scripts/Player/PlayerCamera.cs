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

    protected Vector3 cameraFollowVelocity = Vector3.zero;
    protected Vector3 targetPosition;
    protected Vector3 cameraRotation;
    protected Quaternion targetRotation;

    [Header("Camera speed")]
    public float cameraSmoothTime = 0.2f;

    protected float lookAmountHorizontal;
    protected float lookAmountVertical;
    protected float maximumPivotAngle = 15;
    protected float minimumPivotAngle = -15;

    public void HandleAllCameraMovement()
    {
        //Follow the player 
        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraSmoothTime);
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
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
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
        targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
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