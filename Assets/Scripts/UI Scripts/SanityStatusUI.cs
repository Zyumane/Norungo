using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityStatusUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [Header("Sanity State Pop Ups")]
    [SerializeField] GameObject popUpLucido;
    [SerializeField] GameObject popUpPerturbado;
    [SerializeField] GameObject popUpEnCrisis;
    [SerializeField] GameObject popUpColapso;

    [Header("Fade Options")]
    [SerializeField] float timeBeforeFadeOutBegins = 2f;

    public bool isInPersistentMode = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void DisplaySanityStatus(int playerSanityPercentage)
    {
        DisableAllPopUps();

        switch (playerSanityPercentage)
        {
            case >= 100:
                popUpLucido.SetActive(true);
                break;
            case >= 66 and <= 99:
                popUpPerturbado.SetActive(true);
                break;
            case >= 30 and <= 65:
                popUpEnCrisis.SetActive(true);
                break;
            case >= 1 and <= 29:
                popUpColapso.SetActive(true);
                break;
        }

        if (!isInPersistentMode)
        {
            StartCoroutine(FadeInPopUp());
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }
    
    public void SetPersistentMode(bool active)
    {
        isInPersistentMode = active;

        if (!isInPersistentMode)
        {
            StopAllCoroutines();
            DisableAllPopUps();
            canvasGroup.alpha = 0f;
        }
    }
    
    private void DisableAllPopUps()
    {
        popUpLucido.SetActive(false);
        popUpPerturbado.SetActive(false);
        popUpEnCrisis.SetActive(false);
        popUpColapso.SetActive(false);
    }
    
    IEnumerator FadeInPopUp()
    {
        for (float fade = 0.05f; fade < 1; fade += 0.05f)
        {
            canvasGroup.alpha = fade;

            if (fade > 0.9f)
            {
                StartCoroutine(FadeOutPopUp());
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOutPopUp()
    {
        yield return new WaitForSeconds(timeBeforeFadeOutBegins);

        for (float fade = 1; fade > 0; fade -= 0.05f)
        {
            canvasGroup.alpha = fade;

            if (fade <= 0.05f)
            {
                DisableAllPopUps();
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
