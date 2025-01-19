using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnOptionsButtonClicked()
    {
        OptionsUI.Instance.Show();
    }

    private void OnMainMenuButtonClicked()
    {
        Loader.LoadScene(Loader.Scene.MainMenuScene);
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.TogglePauseGame();
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        
        Hide();
    }

    private void GameManager_OnGameResumed(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
