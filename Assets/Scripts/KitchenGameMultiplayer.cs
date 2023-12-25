using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Services.Authentication;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;



public class KitchenGameMultiplayer : NetworkBehaviour
{
    public const int MAX_PLAYERS_AMOUNT = 4;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public static KitchenGameMultiplayer Instance { get; private set; }


    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedtoJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;


    [SerializeField] private KitchenObjectListSO _KitchenObjectListSO;
    [SerializeField] private List<Color> _PlayerColorList;

    private NetworkList<PlayerData> _PlayerDataNetworkList;

    private string _PlayerName;



    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        _PlayerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + Random.Range(100, 1000));

        _PlayerDataNetworkList = new NetworkList<PlayerData>();
        _PlayerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }

    public string GetPlayerName()
    {
        return _PlayerName;
    }

    public void SetPlayerName(string playerName)
    {
        _PlayerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;

        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_Server_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Server_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;        
        
        NetworkManager.Singleton.StartHost();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_Server_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Server_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;

        NetworkManager.Singleton.StartServer();
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = _PlayerDataNetworkList[playerDataIndex];

        playerData.PlayerName = playerName;

        // We need to copy the PlayerData back into the array since it is a struct.
        _PlayerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = _PlayerDataNetworkList[playerDataIndex];

        playerData.PlayerId = playerId;

        // We need to copy the PlayerData back into the array since it is a struct.
        _PlayerDataNetworkList[playerDataIndex] = playerData;
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedtoJoinGame?.Invoke(this, EventArgs.Empty);
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < _PlayerDataNetworkList.Count; i++)
        {
            PlayerData playerData = _PlayerDataNetworkList[i];

            if (playerData.ClientId == clientId)
            {
                _PlayerDataNetworkList.RemoveAt(i);
                return;
            }
        }
    }

    private void NetworkManager_Server_OnClientConnectedCallback(ulong clientId)
    {
        _PlayerDataNetworkList.Add(new PlayerData
        {
            ClientId = clientId,
            ColorIndex = GetFirstUnusedColorIndex(),
        });

        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    /// <summary>
    /// This is called by the events we setup in the StartHost()/StartServer() methods above. So it only runs on the server.
    /// </summary>
    /// <remarks>
    /// NOTE: This event will not fire unless you enable "Approve Connections" on the NetworkManager object in the game scene.
    /// </remarks>
    /// <param name="connectionApprovalRequest"></param>
    /// <param name="connectionApprovalResponse"></param>
    private void NetworkManager_Server_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scenes.CharacterSelectScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started.";

            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERS_AMOUNT) 
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full.";

            return;
        }


        connectionApprovalResponse.Approved = true;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }
    
    public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO)
    {
        return _KitchenObjectListSO.KitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return _KitchenObjectListSO.KitchenObjectSOList[kitchenObjectSOIndex];
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);

        kitchenObject.DestroySelf();        
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectOnParent();
    }

    public bool IsPlayerConnected(int playerIndex)
    {
        return playerIndex < _PlayerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < _PlayerDataNetworkList.Count; i++)
        {
            if (_PlayerDataNetworkList[i].ClientId == clientId)
                return i;
        }

        return -1;
    }

    public PlayerData GetPlayerDataFromClientId(ulong clientId) 
    { 
        foreach (PlayerData playerData in _PlayerDataNetworkList) 
        {
            if (playerData.ClientId == clientId)
                return playerData;
        }

        return default;
    }

    public PlayerData GetLocalPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return _PlayerDataNetworkList[playerIndex];
    }

    public Color GetPlayerColor(int colorIndex)
    {
        return _PlayerColorList[colorIndex];
    }

    public void ChangePlayerColor(int colorIndex)
    {
        ChangePlayerColorServerRpc(colorIndex);
    }

    public int GetPlayerColorsCount()
    {
        return _PlayerColorList.Count;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerColorServerRpc(int colorIndex, ServerRpcParams serverRpcParams = default)
    {
        if (!IsColorAvailable(colorIndex))
            return;


        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = _PlayerDataNetworkList[playerDataIndex];

        playerData.ColorIndex = colorIndex;

        // We need to copy the PlayerData back into the array since it is a struct.
        _PlayerDataNetworkList[playerDataIndex] = playerData;
    }

    private bool IsColorAvailable(int colorIndex) 
    { 
        foreach (PlayerData playerData in _PlayerDataNetworkList)
        {
            if (playerData.ColorIndex == colorIndex)
                return false;
        }

        return true;
    }

    private int GetFirstUnusedColorIndex()
    {
        for (int i = 0; i < _PlayerColorList.Count; i++)
        {
            if (IsColorAvailable(i))
                return i;
        }

        return -1;
    }

    public void KickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);

        // We have to call this manually, because for some reason the disconnect event doesn't
        // get fired when a player is kicked.
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
    }
}
