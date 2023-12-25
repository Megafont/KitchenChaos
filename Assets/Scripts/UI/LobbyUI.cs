using System.Collections;
using System.Collections.Generic;

using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button _MainMenuButton;
    [SerializeField] private Button _CreateLobbyButton;
    [SerializeField] private Button _QuickJoinButton;
    [SerializeField] private Button _JoinWithCodeButton;
    [SerializeField] private LobbyCreateUI _LobbyCreateUI;
    [SerializeField] private TMP_InputField _LobbyCodeInputField;
    [SerializeField] private TMP_InputField _PlayerNameInputField;
    [SerializeField] private Transform _LobbyListContainer;
    [SerializeField] private Transform _LobbyListItemTemplate;



    private void Awake()
    {
        _MainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _CreateLobbyButton.onClick.AddListener(OnCreateLobbyClicked);
        _QuickJoinButton.onClick.AddListener(OnQuickJoinClicked);
        _JoinWithCodeButton.onClick.AddListener(OnJoinWithCodeClicked);

        _CreateLobbyButton.Select();

        _LobbyListItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _PlayerNameInputField.text = KitchenGameMultiplayer.Instance.GetPlayerName();
        _PlayerNameInputField.onValueChanged.AddListener(OnPlayerNameChanged);

        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.LobbyList);
    }

    private void OnPlayerNameChanged(string newName)
    {
        KitchenGameMultiplayer.Instance.SetPlayerName(newName);
    }

    private void OnMainMenuClicked()
    {
        KitchenGameLobby.Instance.LeaveLobby();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void OnCreateLobbyClicked()
    {
        if (string.IsNullOrWhiteSpace(_PlayerNameInputField.text))
            return;

        _LobbyCreateUI.Show();
    }

    private void OnQuickJoinClicked()
    {
        if (string.IsNullOrWhiteSpace(_PlayerNameInputField.text))
            return;

        KitchenGameLobby.Instance.QuickJoin();
    }

    private void OnJoinWithCodeClicked()
    {
        if (string.IsNullOrWhiteSpace(_PlayerNameInputField.text) ||
            string.IsNullOrWhiteSpace(_LobbyCodeInputField.text))
        {
            return;
        }

        KitchenGameLobby.Instance.JoinWithCode(_LobbyCodeInputField.text);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        // Clear all UI items from the list.
        foreach (Transform child in _LobbyListContainer)
        {
            if (child == _LobbyListItemTemplate)
                continue;

            Destroy(child.gameObject);
        }

        // Create UI for each lobby in the list.
        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_LobbyListItemTemplate, _LobbyListContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);

        }
    }
}
