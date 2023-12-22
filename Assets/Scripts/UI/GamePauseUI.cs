using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
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
        KitchenGameManager.Instance.OnLocalGamePaused += KitchenGameManager_OnLocalGamePaused;        
        KitchenGameManager.Instance.OnLocalGameUnpaused += KitchenGameManager_OnLocalGameUnpaused;

        Hide();
    }

    private void OnResumeClicked()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }

    private void OnOptionsClicked()
    {
        Hide();
        OptionsUI.Instance.Show(Show); // Pass in this class' Show() method.
    }

    private void OnMainMenuClicked()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void KitchenGameManager_OnLocalGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnLocalGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        _ResumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
