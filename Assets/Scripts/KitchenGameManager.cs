using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;


    [SerializeField] private float _GamePlayingTimerMax = 300f;
        
        
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

    private bool _IsLocalPlayerReady;
    private bool _IsGamePaused;
    private Dictionary<ulong, bool> _PlayerReadyDictionary;



    private void Awake()
    {
        Instance = this;

        _PlayerReadyDictionary = new Dictionary<ulong, bool>();
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
        _IsGamePaused = !_IsGamePaused;

        if (_IsGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
