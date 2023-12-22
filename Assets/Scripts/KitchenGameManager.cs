using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;


    [SerializeField] private float _GamePlayingTimerMax = 300f;
    [SerializeField] private Transform _PlayerPrefab;
        

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }


    private NetworkVariable<State> _State = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> _CountdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> _GamePlayingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<bool> _IsGamePaused = new NetworkVariable<bool>(false);

    private bool _IsLocalPlayerReady;
    private bool _IsLocalGamePaused;
    private Dictionary<ulong, bool> _PlayerReadyDictionary;
    private Dictionary<ulong, bool> _PlayerPausedDictionary;
    private bool _AutoTestGamePausedState;



    private void Awake()
    {
        Instance = this;

        _PlayerReadyDictionary = new Dictionary<ulong, bool>();
        _PlayerPausedDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    /// <summary>
    /// This is like Awake() or Start(), but for NetworkObjects.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        _State.OnValueChanged += State_OnValueChanged;
        _IsGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    /// <summary>
    /// This event handler runs when the scene is loaded.
    /// </summary>
    /// <remarks>
    /// NOTE: This event handler is called by NetworkManager.Singleton.SceneManager, not the regular Unity SceneManager.
    /// </remarks>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="clientsCompleted"></param>
    /// <param name="clientsTimedOut"></param>
    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        // Spawn the players.
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(_PlayerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    /// <summary>
    /// We are using this event to do an extra check of the game's paused state.
    /// That way if the only player that's paused leaves the game or disconnects,
    /// it won't stay paused for the others.
    /// </summary>
    /// <param name="clientId"></param>
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        _AutoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (_IsGamePaused.Value) 
        {
            Time.timeScale = 0f;

            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_State.Value == State.WaitingToStart) 
        {
            _IsLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
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
            _State.Value = State.CountdownToStart;
        }
    }

    private void Update()
    {
        // Do not run the update logic if this is not the server/host.
        if (!IsServer)
            return;


        switch (_State.Value)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                _CountdownToStartTimer.Value -= Time.deltaTime;

                if (_CountdownToStartTimer.Value <= 0f)
                {
                    _State.Value = State.GamePlaying;
                    _GamePlayingTimer.Value = _GamePlayingTimerMax;
                }

                break;

            case State.GamePlaying:
                _GamePlayingTimer.Value -= Time.deltaTime;

                if (_GamePlayingTimer.Value <= 0f)
                {
                    _State.Value = State.GameOver;
                }

                break;

            case State.GameOver:
                break;
        }

    }

    private void LateUpdate()
    {
        if (_AutoTestGamePausedState)
        {
            _AutoTestGamePausedState = false;

            TestGamePausedState();
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public bool IsGamePlaying()
    { 
        return _State.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _State.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _CountdownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return _State.Value == State.GameOver;
    }

    public bool IsWaitingToStart()
    {
        return _State.Value == State.WaitingToStart;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1f - (_GamePlayingTimer.Value / _GamePlayingTimerMax);
    }

    public bool IsLocalPlayerReady()
    {
        return _IsLocalPlayerReady;
    }

    public void TogglePauseGame()
    {
        _IsLocalGamePaused = !_IsLocalGamePaused;

        if (_IsLocalGamePaused)
        {
            PauseGameServerRpc();

            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();

            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _PlayerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _PlayerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePausedState();
    }

    /// <summary>
    /// Checks if all players are unpaused and updates multiplayer paused state.
    /// </summary>
    /// <remarks>
    /// NOTE: I had a not the server exception being raised by this function accessing the ConnectedClientIds field
    ///       of the NetworkManager. It turns out this was just caused because while testing I was just stopping the
    ///       game in the Unity Editor rather than hitting Esc to pause it, and clicking the Main Menu button.
    /// </remarks>
    private void TestGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (_PlayerPausedDictionary.ContainsKey(clientId) && _PlayerPausedDictionary[clientId])
            {
                // This player is paused
                _IsGamePaused.Value = true;
                return;
            }
        } // end foreach

        // All players are unpaused
        _IsGamePaused.Value = false;
    }
}
