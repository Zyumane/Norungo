using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFeedbackUI : MonoBehaviour
{
    private static SaveFeedbackUI _instance;
    public static SaveFeedbackUI Instance => _instance;

    [Header("References")]
    public CanvasGroup canvasGroup;
    public Image accentBar;
    public TextMeshProUGUI titleText;

    [Header("Settigs")]
    public float displayDuration = 5f;
    public float fadeDuration = 1.4f;

    [Header("Colors")]
    public Color colorExitu = new Color(0.22f, 0.59f, 0.35f);
    public Color colorError = new Color(0.75f, 0.18f, 0.18f);

    private Coroutine _activeRoutine;

    private void Awake()
    {
        if (_instance != null && _instance != this) 
        { Destroy(this); return; }
        _instance = this;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowExitu(string message)
    {
        ShowText(message, colorExitu);
    }

    public void ShowError(string message)
    {
        ShowText(message, colorError);
    }

    private void ShowText(string message, Color accent)
    {
        if(_activeRoutine != null)
        {
            StopCoroutine(_activeRoutine);
        }

        titleText.text = message;
        accentBar.color = accent;
        _activeRoutine = StartCoroutine(feedbackCycle());
    }

    private IEnumerator feedbackCycle()
    {
        Debug.Log("[FEEDBACK] Corrutina iniciada.");
        float timer = 0f;

        while(timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(displayDuration);

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

}
