using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


public class PlayerAnimator : NetworkBehaviour
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
        // Run the update code only if this is the local player.
        if (!IsOwner)
        {
            return;
        }

        _Animator.SetBool(IS_WALKING, _Player.IsWalking());
    }
}
