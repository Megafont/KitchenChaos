using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    [SerializeField] private float _GamePlayingTimerMax = 10f;
        
        
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }


    private State _State;
    private float _WaitingToStartTimer = 1f;
    private float _CountdownToStartTimer = 3f;
    private float _GamePlayingTimer;
    private bool _IsGamePaused;



    private void Awake()
    {
        Instance = this;

        _State = State.WaitingToStart;    
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void Update()
    {
        switch (_State)
        {
            case State.WaitingToStart:
                _WaitingToStartTimer -= Time.deltaTime;

                if (_WaitingToStartTimer <= 0f)
                {
                    _State = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.CountdownToStart:
                _CountdownToStartTimer -= Time.deltaTime;

                if (_CountdownToStartTimer <= 0f)
                {
                    _State = State.GamePlaying;
                    _GamePlayingTimer = _GamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.GamePlaying:
                _GamePlayingTimer -= Time.deltaTime;

                if (_GamePlayingTimer <= 0f)
                {
                    _State = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.GameOver:
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
        }

    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public bool IsGamePlaying()
    { 
        return _State == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _State == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _CountdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return _State == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1f - (_GamePlayingTimer / _GamePlayingTimerMax);
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
            OnGameUnpaused.Invoke(this, EventArgs.Empty);
        }
    }
}
