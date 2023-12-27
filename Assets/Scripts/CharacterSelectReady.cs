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

    public void TogglePlayerReady()
    {
        TogglePlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void TogglePlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        _PlayerReadyDictionary[clientId] = !IsPlayerReady(clientId);
        TogglePlayerReadyClientRpc(clientId, _PlayerReadyDictionary[clientId]);

        bool allClientsReady = true;
        foreach (ulong curClientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_PlayerReadyDictionary.ContainsKey(curClientId) || !_PlayerReadyDictionary[curClientId])
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
    private void TogglePlayerReadyClientRpc(ulong clientId, bool isReady)
    {
        _PlayerReadyDictionary[clientId] = isReady;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return _PlayerReadyDictionary.ContainsKey(clientId) && _PlayerReadyDictionary[clientId];
    }
}
