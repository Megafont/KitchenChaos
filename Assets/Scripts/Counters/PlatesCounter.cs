using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO _PlateKitchenObjectSO;
    [SerializeField] [Min(1f)] private float _SpawnPlateTimerMax = 4f;
    [SerializeField] [Min(1f)] private int _PlatesSpawnedAmountMax = 4;


    private float _SpawnPlateTimer;
    private int _PlatesSpawnedAmount;


    private void Update()
    {
        _SpawnPlateTimer += Time.deltaTime;
        if (_SpawnPlateTimer > _SpawnPlateTimerMax)
        {
            _SpawnPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && _PlatesSpawnedAmount < _PlatesSpawnedAmountMax)
            {
                _PlatesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if (_PlatesSpawnedAmount > 0)
            {
                // There's at least one plate here
                _PlatesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(_PlateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
