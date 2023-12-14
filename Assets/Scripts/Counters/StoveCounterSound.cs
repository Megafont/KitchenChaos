using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _StoveCounter;
    private AudioSource _AudioSource;



    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _StoveCounter.OnStateChanged += _StoveCounter_OnStateChanged;
    }

    private void _StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Fried;
        if (playSound)
            _AudioSource.Play();
        else
            _AudioSource.Stop();
    }
}
