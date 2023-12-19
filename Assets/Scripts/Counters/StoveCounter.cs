using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using static CuttingCounter;


public class StoveCounter : BaseCounter, IHasProgress
{
    // Once the meat is cooked, this much progress toward burning must be made before
    // the burning warning appears.
    public const float STOVE_BURNING_WARNING_TIME = 0.5f;


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs
    {
        public State State;
    }


    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }


    [SerializeField] private FryingRecipeSO[] _FryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _BurningRecipeSOArray;


    private NetworkVariable<State> _State = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> _FryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> _BurningTimer = new NetworkVariable<float>(0f);

    private FryingRecipeSO _FryingRecipeSO;
    private BurningRecipeSO _BurningRecipeSO;



    // This is like Awake() or Start(), but for NetworkObjects.
    public override void OnNetworkSpawn()
    {
        _FryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        _BurningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        _State.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        // This line is in case the client hasn't received the object yet.
        float fryingTimerMax = _FryingRecipeSO != null ? _FryingRecipeSO.FryingTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            ProgressNormalized = _FryingTimer.Value / fryingTimerMax
        });
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        // This line is in case the client hasn't received the object yet.
        float burningTimerMax = _BurningRecipeSO != null ? _BurningRecipeSO.BurningTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            ProgressNormalized = _BurningTimer.Value / burningTimerMax
        });
    }

    private void State_OnValueChanged(State previousState, State newState)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
        {
            State = _State.Value
        });


        if (_State.Value == State.Burned || _State.Value == State.Idle)
        {
            // Reset timer progress.
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = 0f
            });
        }
    }

    private void Update()
    {
        // Only run the update logic if this is the server.
        if (!IsServer)
            return;


        if (HasKitchenObject())
        {

            switch (_State.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _FryingTimer.Value += Time.deltaTime;

                    if (_FryingTimer.Value > _FryingRecipeSO.FryingTimerMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_FryingRecipeSO.Output, this);

                        _State.Value = State.Fried;
                        _BurningTimer.Value = 0f;

                        SetBurningRecipeSOClientRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));
                    }
                    break;
                case State.Fried:
                    _BurningTimer.Value += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _BurningTimer.Value / _BurningRecipeSO.BurningTimerMax
                    });

                    if (_BurningTimer.Value > _BurningRecipeSO.BurningTimerMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_BurningRecipeSO.Output, this);

                        _State.Value = State.Burned;
                        _BurningTimer.Value = 0f;
                    }
                    break;
                case State.Burned:
                    break;
            }

        }

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be fried.
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicPlaceObjectOnCounterServerRpc(
                        KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO())
                    );
                }
            }
            else
            {
                // Player has nothing

            }
        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        SetStateIdleServerRpc();
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                SetStateIdleServerRpc();

            }
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        _State.Value = State.Idle;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        _FryingTimer.Value = 0f;
        _State.Value = State.Frying;

        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _FryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _BurningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null)
            return fryingRecipeSO.Output;
        else
            return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _FryingRecipeSOArray)
        {
            if (fryingRecipeSO.Input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _BurningRecipeSOArray)
        {
            if (burningRecipeSO.Input == inputKitchenObjectSO)
                return burningRecipeSO;
        }

        return null;
    }

    public bool IsFried()
    {
        return _State.Value == State.Fried;
    }
}
