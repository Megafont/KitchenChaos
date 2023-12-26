using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int _PlayerIndex;
    [SerializeField] private TextMeshPro _PlayerNameText;
    [SerializeField] private GameObject _ReadyText;
    [SerializeField] private PlayerVisual _PlayerVisual;
    [SerializeField] private Button _KickButton;



    private void Awake()
    {
        _KickButton.onClick.AddListener(OnKickClicked);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        //_KickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        UpdateKickButtonState();
        
        UpdatePlayer();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
    }

    private void OnKickClicked()
    {
        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_PlayerIndex);
        KitchenGameLobby.Instance.KickPlayer(playerData.PlayerId.ToString());
        KitchenGameMultiplayer.Instance.KickPlayer(playerData.ClientId);
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void KitchenGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (KitchenGameMultiplayer.Instance.IsPlayerConnected(_PlayerIndex))
        {
            Show();

            UpdateKickButtonState();

            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_PlayerIndex);
            _ReadyText.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.ClientId));

            _PlayerNameText.text = playerData.PlayerName.ToString();
            //Debug.Log("NAME: " + playerData.PlayerName.ToString() + "    CId: " + playerData.ClientId + "    PId: " + playerData.PlayerId);

            _PlayerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.ColorIndex));
        }
        else
        {
            Hide();
        }
    }

    private void UpdateKickButtonState()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            _KickButton.gameObject.SetActive(false);
            return;
        }


        // If the index of this player visual is higher than the valid range for the number of players currently in the game, then simply return since we don't need to update the KickButton's state since it will be invisible anyway.
        if (_PlayerIndex >= NetworkManager.Singleton.ConnectedClientsIds.Count)
            return;


        // Show the kick buttons only if this is player is not the host AND if this code is running on the server.
        // In other words, if this code is running on the host/server, all players will have the kick button above their heads except the host.
        // If this code is running on a client, no players will have a kick button, as clients are not allowed to kick players.
        if (KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_PlayerIndex).PlayerId != KitchenGameMultiplayer.Instance.GetLocalPlayerData().PlayerId)
            _KickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        else
            _KickButton.gameObject.SetActive(false);
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
