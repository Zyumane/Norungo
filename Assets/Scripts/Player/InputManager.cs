using System;
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
    public bool sanityHUDToggleInput;
    public bool navigateSlotUsed = false;
    public bool useActiveSlotInput;

    [Header("Inventory Action Buttons")]
    public bool useRightHandInput;
    public bool useLeftHandInput;
    public bool useBeltSlot1Input;
    public bool useBeltSlot2Input;
    public bool useBeltSlot3Input;
    public Vector2 navigateSlotsInput;


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
            playerControls.Player_Actions.Sanity_HUD_Toggle.performed += i => sanityHUDToggleInput = true;
            playerControls.Player_Actions.UseRightHand.performed += i => useRightHandInput = true;
            playerControls.Player_Actions.UseLeftHand.performed += i => useLeftHandInput = true;
            playerControls.Player_Actions.UseBeltSlot1.performed += i => useBeltSlot1Input = true;
            playerControls.Player_Actions.UseBeltSlot2.performed += i => useBeltSlot2Input = true;
            playerControls.Player_Actions.UseBeltSlot3.performed += i => useBeltSlot3Input = true;
            playerControls.Player_Actions.NavigateSlots.performed += i => navigateSlotsInput = i.ReadValue<Vector2>();
            playerControls.Player_Actions.NavigateSlots.canceled += i => navigateSlotsInput = Vector2.zero;
            playerControls.Player_Actions.UseItem.performed += i => useActiveSlotInput = true;
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
        HandleSanityHUDToggleInput();
        HandleUseItemInput();
        HandleNavigationSlotInput();
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
        if(playerManager.isPerformingAction)
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

    private void HandleSanityHUDToggleInput()
    {
        if(sanityHUDToggleInput)
        {
            sanityHUDToggleInput = false;
            playerManager.playerUI.ToggleSanityHUD();
        }
    }

    private void HandleUseItemInput()
    {

        if (useRightHandInput)
        {
            useRightHandInput = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.rightHand,
                playerManager.playerSanityManager);
        }

        if (useLeftHandInput)
        {
            useLeftHandInput = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.leftHand,
                playerManager.playerSanityManager);
        }

        if (useBeltSlot1Input)
        {
            useBeltSlot1Input = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.beltSlot1,
                playerManager.playerSanityManager);
        }

        if (useBeltSlot2Input)
        {
            useBeltSlot2Input = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.beltSlot2,
                playerManager.playerSanityManager);
        }

        if (useBeltSlot3Input)
        {
            useBeltSlot3Input = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.beltSlot3,
                playerManager.playerSanityManager);
        }
        if (useActiveSlotInput)
        {
            useActiveSlotInput = false;
            playerManager.playerInventoryManager.UseItem(
                playerManager.playerInventoryManager.GetActiveItem(),
                playerManager.playerSanityManager);
        }

    }

    private void HandleNavigationSlotInput()
    {
        PlayerInventoryManager inventoryA = playerManager.playerInventoryManager;

        if(navigateSlotsInput == Vector2.zero)
        {
            navigateSlotUsed = false;
            return;
        }

        if (navigateSlotUsed)
        {
            return;
        }

        navigateSlotUsed = true;
        bool inBelt = inventoryA.activeSlotIndex >= 2;

        if (navigateSlotsInput.y < -0.5f)
        {
            if (!inBelt && inventoryA.gotBelt)
                inventoryA.SetActiveSlot(2);
        }
        else if (navigateSlotsInput.y > 0.5f)
        {
            if (inBelt)
            {
                inventoryA.SetActiveSlot(inventoryA.lastHandIndex);
            }
        }
        else if(navigateSlotsInput.x > 0.5f)
        {
            if(!inBelt)
            {
                inventoryA.SetActiveSlot(Mathf.Clamp(inventoryA.activeSlotIndex + 1, 0, 1));
            }
            else
            {
                if(inventoryA.activeSlotIndex == 2)
                {
                    inventoryA.SetActiveSlot(4);
                }
                else
                {
                    inventoryA.SetActiveSlot(inventoryA.activeSlotIndex - 1);
                }
            }
        }
        else if(navigateSlotsInput.x < -0.5f)
        {
            if(!inBelt)
            {
                inventoryA.SetActiveSlot(Mathf.Clamp(inventoryA.activeSlotIndex - 1, 0, 1));
            }
            else
            {
                if(inventoryA.activeSlotIndex == 4)
                {
                    inventoryA.SetActiveSlot(2);
                }
                else
                {
                    inventoryA.SetActiveSlot(inventoryA.activeSlotIndex + 1);
                }
            }
        }
        playerManager.playerUI.UpdateActiveSlotIndicator(inventoryA.activeSlotIndex);
    }
}

/*Scrap code
        //Belt Tool pouch acces gamepad down
        if(navigateSlotsInput.y < -0.5f)
        {
            if (!inBelt && inventoryA.gotBelt)
                inventoryA.SetActiveSlot(2);
            return;
        }
    //Jump from belt to hand
        if (navigateSlotsInput.y > 0.5f)
        {
            if (inBelt)
            {
                inventoryA.SetActiveSlot(inventoryA.lastHandIndex);
            }
            return;
        }

        //gamepad crosspad right
        if (navigateSlotsInput.x > 0.5f)
        {
            if (!inBelt)
            {
                inventoryA.SetActiveSlot(Mathf.Clamp(inventoryA.activeSlotIndex + 1, 0, 1));
            }
            else
            {
                if (inventoryA.activeSlotIndex == 4)
                    inventoryA.SetActiveSlot(2);
                else
                    inventoryA.SetActiveSlot(inventoryA.activeSlotIndex + 1);
            }
            return;
        }

        //Gamepad crosspad left
        if (navigateSlotsInput.x < -0.5f)
        {
            if (!inBelt)
            {
                inventoryA.SetActiveSlot(Mathf.Clamp(inventoryA.activeSlotIndex - 1, 0, 1));
            }
            else
            {
                if (inventoryA.activeSlotIndex == 2)
                    inventoryA.SetActiveSlot(4);
                else
                    inventoryA.SetActiveSlot(inventoryA.activeSlotIndex - 1);
            }
            return;
        }
 
 */
