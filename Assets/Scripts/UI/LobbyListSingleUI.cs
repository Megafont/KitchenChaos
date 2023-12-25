using System.Collections;
using System.Collections.Generic;

using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LobbyListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _LobbyNameText;


    private Lobby _Lobby;



    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        KitchenGameLobby.Instance.JoinWithId(_Lobby.Id);
    }

    public void SetLobby(Lobby lobby)
    {
        _Lobby = lobby;

        _LobbyNameText.text = $"{_Lobby.Name}    {_Lobby.Players.Count}/{_Lobby.MaxPlayers}";
    }

}
