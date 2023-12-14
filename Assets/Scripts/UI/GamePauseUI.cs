using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _ResumeButton;
    [SerializeField] private Button _OptionsButton;
    [SerializeField] private Button _MainMenuButton;


    private void Awake()
    {
        _ResumeButton.onClick.AddListener(OnResumeClicked);
        _OptionsButton.onClick.AddListener(OnOptionsClicked);
        _MainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;        
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    private void OnResumeClicked()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }

    private void OnOptionsClicked()
    {
        OptionsUI.Instance.Show();
    }

    private void OnMainMenuClicked()
    {
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
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
