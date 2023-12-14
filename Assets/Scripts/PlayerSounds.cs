using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerSounds : MonoBehaviour
{    
    private Player _Player;
    private float _FootStepTimer;
    private float _FootStepTimerMax = 0.1f;



    private void Awake()
    {
        _Player = GetComponent<Player>();
    }

    private void Update()
    {
        _FootStepTimer -= Time.deltaTime;
        if (_FootStepTimer <= 0)
        {
            _FootStepTimer = _FootStepTimerMax;

            if (_Player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepSound(transform.position, volume);
            }
        }
    }
}
