using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _StoveCounter;
    
    
    private AudioSource _AudioSource;
    private float _WarningSoundTimer;
    private float _WarningSoundTimerMax = 0.2f;
    private bool _PlayWarningSound;



    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _StoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _StoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if (_PlayWarningSound)
        {
            _WarningSoundTimer -= Time.deltaTime;
            if (_WarningSoundTimer < 0)
            {
                _WarningSoundTimer = _WarningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(transform.position);
            }
        }
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        _PlayWarningSound = _StoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Fried;
        if (playSound)
            _AudioSource.Play();
        else
            _AudioSource.Stop();
    }
}
