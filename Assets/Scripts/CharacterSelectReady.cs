using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }


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
            Loader.LoadSceneMultiplayer(Loader.Scenes.GameScene);
        }
    }
}
