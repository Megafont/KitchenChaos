using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";


    [SerializeField] private ContainerCounter _ContainerCounter;


    private Animator _Animator;



    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _ContainerCounter.OnPlayerGrabbedObject += _ContainerCounter_OnPlayerGrabbedObject;
    }

    private void _ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        _Animator.SetTrigger(OPEN_CLOSE);
    }
}
