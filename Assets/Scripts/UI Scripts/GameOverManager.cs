using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup gameOverCanvasGroup;
    public Button retryButton;
    public Button loadButton;
    public Button mainMenuButton;

    [Header("Settings")]
    public float fadeDuration = 1.5f;

    private static GameOverManager _instance;
    public static GameOverManager Instance => _instance;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetry);
        loadButton.onClick.AddListener(OnLoadGame);
        mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    public void TriggerGameOver()
    {
        PlayerManager player = FindObjectOfType<PlayerManager>();
        if (player != null)
            player.isGameOver = true;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        Time.timeScale = 0f;
    }

    private void OnRetry()
    {
        // STUB — conectar a JsonUtility en sesión de guardado
        Debug.Log("[GAME OVER] Reintentar — pendiente de sistema de guardado.");

    }

    private void OnLoadGame()
    {
        // STUB — conectar a JsonUtility en sesión de guardado
        Debug.Log("[GAME OVER] Cargar partida — pendiente de sistema de guardado.");
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
