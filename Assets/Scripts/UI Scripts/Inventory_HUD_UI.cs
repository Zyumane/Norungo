using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Inventory_HUD_UI : MonoBehaviour
{
    [Header("Slot Icons")]
    [SerializeField] GameObject iconRightHand;
    [SerializeField] GameObject iconLeftHand;
    [SerializeField] GameObject iconBeltSlot1;
    [SerializeField] GameObject iconBeltSlot2;
    [SerializeField] GameObject iconBeltSlot3;

    [Header("Pulse Settings")]
    [SerializeField] float pulseMin = 0.4f;
    [SerializeField] float pulseMax = 1f;
    [SerializeField] float pulseSpeed = 0.7f;

    [Header("Hand Slot Rect Transforms")]
    [SerializeField] RectTransform rightHandRect;
    [SerializeField] RectTransform leftHandRect;

    [Header("Belt Animation Settings")]
    [SerializeField] float handsTargetY = 275f;
    [SerializeField] float beltAnimDuration = 0.5f;

    [ContextMenu("Test Unlock Belt")]
    public void TestUnlockBelt()
    {
        PlayUnlockBeltAnimation();
    }

    private GameObject[] slots;
    private CanvasGroup[] canvasGroups;
    private Coroutine pulseCoroutine;
    private bool beltUnlocked = false;

    private void Awake()
    {
        slots = new GameObject[]
        {
            iconRightHand,
            iconLeftHand,
            iconBeltSlot1,
            iconBeltSlot2,
            iconBeltSlot3
        };

        canvasGroups = new CanvasGroup[slots.Length];
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    canvasGroups[i] = slots[i].GetComponent<CanvasGroup>();
                }
            }
        }
    }

    private void Start()
    {
        SetActiveSlotIndicator(0);    
    }

    public void SetActiveSlotIndicator(int activeIndex)
    {
        

        //pause the pulse effect
        if(pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        //Reboot all the slots to alpha without pulse
        for (int i = 0; i < slots.Length; i++)
        {
            if (canvasGroups[i] != null)
            {
                // No toca los Belt Slots si el cinturon no esta desbloqueado
                if (i >= 2 && !beltUnlocked)
                    continue;
                canvasGroups[i].alpha = pulseMin;
            }
        }

        //Activates the alpha in the active slot
        if (activeIndex >= 0 && activeIndex < slots.Length)
        {
            if (canvasGroups[activeIndex] != null)
                pulseCoroutine = StartCoroutine(PulseSlot(canvasGroups[activeIndex]));
        }
    }

    private IEnumerator PulseSlot(CanvasGroup canvasGroupT)
    {
        while (true)
        {
            //ups effect
            for(float timer = 0f; timer < 1f; timer += Time.deltaTime / pulseSpeed)
            {
                canvasGroupT.alpha = Mathf.Lerp(pulseMin, pulseMax, timer);
                yield return null;
            }
            canvasGroupT.alpha = pulseMax;

            //down effect
            for(float timerL = 0f; timerL < 1f; timerL += Time.deltaTime / pulseSpeed)
            {
                canvasGroupT.alpha = Mathf.Lerp(pulseMax, pulseMin, timerL);
                yield return null;
            }
            canvasGroupT.alpha = pulseMin;
        }
    }

    public void PlayUnlockBeltAnimation()
    {
        beltUnlocked = true;
        float duration = 2.5f;

        rightHandRect.DOAnchorPosY(handsTargetY, beltAnimDuration).SetEase(Ease.OutQuart);
        leftHandRect.DOAnchorPosY(handsTargetY, beltAnimDuration).SetEase(Ease.OutQuart);

        canvasGroups[2].DOFade(1f, duration).SetEase(Ease.OutQuart);
        canvasGroups[3].DOFade(1f, duration).SetEase(Ease.OutQuart);
        canvasGroups[4].DOFade(1f, duration).SetEase(Ease.OutQuart);
    }
}
