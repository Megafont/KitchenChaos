using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";


    [SerializeField] private CuttingCounter _CuttingCounter;


    private Animator _Animator;



    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _CuttingCounter.OnCut += _ContainerCounter_OnCut;
    }

    private void _ContainerCounter_OnCut(object sender, System.EventArgs e)
    {
        _Animator.SetTrigger(CUT);
    }
}
