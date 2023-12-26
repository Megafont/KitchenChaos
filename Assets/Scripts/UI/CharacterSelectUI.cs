using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _MainMenuButton;
    [SerializeField] private Button _ReadyButton;
    [SerializeField] private TextMeshProUGUI _LobbyNameText;
    [SerializeField] private TextMeshProUGUI _LobbyCodeText;



    private void Awake()
    {
        _MainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _ReadyButton.onClick.AddListener(OnReadyClicked);

        _ReadyButton.Select();
    }

    private void Start()
    {
        Lobby lobby = KitchenGameLobby.Instance.GetLobby();

        if (KitchenGameMultiplayer.PlayMultiplayer)
        {
            _LobbyNameText.text = $"Lobby Name: {lobby.Name}";
            _LobbyCodeText.text = $"Lobby Code: {lobby.LobbyCode}";
        }
        else
        {
            _LobbyNameText.gameObject.SetActive(false);
            _LobbyCodeText.gameObject.SetActive(false);
        }
    }

    private void OnMainMenuClicked()
    {
        KitchenGameLobby.Instance.LeaveLobby();
        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void OnReadyClicked()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }
}
