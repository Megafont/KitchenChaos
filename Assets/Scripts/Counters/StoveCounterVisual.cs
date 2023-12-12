using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter _StoveCounter;
    [SerializeField] private GameObject _StoveOnGameObject;
    [SerializeField] private GameObject _ParticlesGameObject;


    private void Start()
    {
        _StoveCounter.OnStateChanged += _StoveCounter_OnStateChanged;   
    }

    private void _StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Fried;

        _StoveOnGameObject.SetActive(showVisual);
        _ParticlesGameObject.SetActive(showVisual);
    }
}
