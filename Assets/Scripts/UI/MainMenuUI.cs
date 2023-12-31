using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _MultiplayerButton;
    [SerializeField] private Button _SingleplayerButton;
    [SerializeField] private Button _QuitButton;



    private void Awake()
    {
        _MultiplayerButton.onClick.AddListener(MultiplayerClicked);
        _SingleplayerButton.onClick.AddListener(SingleplayerClicked);
        _QuitButton.onClick.AddListener(QuitClicked);

        // This is needed because the GamePauseUI script doesn't reset this value first when the MainMenuButton is clicked.
        // Doing it here instead ensures the player will never see stuff start moving again before the loading screen appears.
        Time.timeScale = 1.0f;

        _MultiplayerButton.Select();
    }
   
    private void MultiplayerClicked()
    {
        KitchenGameMultiplayer.PlayMultiplayer = true;
        Loader.LoadScene(Loader.Scenes.LobbyScene);
    }

    private void SingleplayerClicked()
    {
        KitchenGameMultiplayer.PlayMultiplayer = false;
        Loader.LoadScene(Loader.Scenes.LobbyScene);
    }

    private void QuitClicked()
    {
        Application.Quit();
    }
}
