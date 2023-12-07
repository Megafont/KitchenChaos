using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";


    [SerializeField] private Player _Player;


    private Animator _Animator;


    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _Animator.SetBool(IS_WALKING, _Player.IsWalking());
    }
}
