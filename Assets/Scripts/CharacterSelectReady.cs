using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }


    public event EventHandler OnReadyChanged;


    private Dictionary<ulong, bool> _PlayerReadyDictionary;


    private void Awake()
    {
        Instance = this;

        _PlayerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        _PlayerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_PlayerReadyDictionary.ContainsKey(clientId) || !_PlayerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        } // end foreach

        if (allClientsReady)
        {
            KitchenGameLobby.Instance.DeleteLobby();
            Loader.LoadSceneMultiplayer(Loader.Scenes.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        _PlayerReadyDictionary[clientId] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return _PlayerReadyDictionary.ContainsKey(clientId) && _PlayerReadyDictionary[clientId];
    }
}
