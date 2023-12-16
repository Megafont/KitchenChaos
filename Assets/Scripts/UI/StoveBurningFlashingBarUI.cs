using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StoveBurningFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";


    [SerializeField] private StoveCounter _StoveCounter;


    private Animator _Animator;



    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _StoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        _Animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = _StoveCounter.IsFried() && e.ProgressNormalized >= StoveCounter.STOVE_BURNING_WARNING_TIME;

        _Animator.SetBool(IS_FLASHING, show);
    }
}
