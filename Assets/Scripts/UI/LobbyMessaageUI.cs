using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System;


public class LobbyMessaageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _MessageText;
    [SerializeField] private Button _CloseButton;


    private void Awake()
    {
        _CloseButton.onClick.AddListener(OnCloseClicked);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedtoJoinGame += KitchenGameMultiplayer_OnFailedtoJoinGame;
        
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void KitchenGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating lobby...");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create lobby!");
    }

    private void KitchenGameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining lobby...");
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join lobby!");
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find a lobby to Quick Join!");
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedtoJoinGame -= KitchenGameMultiplayer_OnFailedtoJoinGame;
    }

    private void KitchenGameMultiplayer_OnFailedtoJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
            ShowMessage("Failed to connect (timed out).");
        else
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
    }

    private void ShowMessage(string message)
    {
        _MessageText.text = message;

        Show();
    }

    private void OnCloseClicked()
    {
        Hide();
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
