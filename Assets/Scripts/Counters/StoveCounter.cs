using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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


    private State _State;
    private float _FryingTimer;
    private float _BurningTimer;
    private FryingRecipeSO _FryingRecipeSO;
    private BurningRecipeSO _BurningRecipeSO;



    private void Start()
    {
        _State = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {

            switch (_State)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _FryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                    { 
                        ProgressNormalized = _FryingTimer / _FryingRecipeSO.FryingTimerMax 
                    });

                    if (_FryingTimer > _FryingRecipeSO.FryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_FryingRecipeSO.Output, this);

                        _State = State.Fried;
                        _BurningTimer = 0f;
                        _BurningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { State = _State });
                    }
                    break;
                case State.Fried:
                    _BurningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _BurningTimer / _BurningRecipeSO.BurningTimerMax
                    });

                    if (_BurningTimer > _BurningRecipeSO.BurningTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_BurningRecipeSO.Output, this);

                        _State = State.Burned;
                        _BurningTimer = 0f;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { State = _State });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = 0f
                        });
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
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    _FryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    _State = State.Frying;
                    _FryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { State = _State });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _FryingTimer / _FryingRecipeSO.FryingTimerMax
                    });
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

                        _State = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { State = _State });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                _State = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { State = _State });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    ProgressNormalized = 0f
                });
            }
        }

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
        return _State == State.Fried;
    }
}
