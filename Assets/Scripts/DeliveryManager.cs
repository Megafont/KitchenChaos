using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;

using Random = UnityEngine.Random;


public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler<RecipeDeliveredEventHandler> OnRecipeSuccess;
    public event EventHandler<RecipeDeliveredEventHandler> OnRecipeFailed;

    public class RecipeDeliveredEventHandler
    {
        public Vector3 DeliveryCounterPosition;
    }

    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO _RecipeListSO;
    [SerializeField] private float _SpawnRecipeTimerMax = 4f;
    [SerializeField] private int _WaitingRecipeMax = 4;


    private List<RecipeSO> _WaitingRecipeSOList;
    private float _SpawnRecipeTimer;
    private int _SuccessfulRecipeAmount;



    private void Awake()
    {
        Instance = this;

        _SpawnRecipeTimer = _SpawnRecipeTimerMax;

        _WaitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        // Run the update logic only if this is the host/server.
        if (!IsServer)
        {
            return;
        }

        _SpawnRecipeTimer -= Time.deltaTime;
        if (_SpawnRecipeTimer <= 0f)
        {
            _SpawnRecipeTimer = _SpawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && _WaitingRecipeSOList.Count < _WaitingRecipeMax)
            {
                int waitingRecipeSOIndex = Random.Range(0, _RecipeListSO.RecipeSOList.Count);
                RecipeSO waitingRecipeSO = _RecipeListSO.RecipeSOList[waitingRecipeSOIndex];

                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = _RecipeListSO.RecipeSOList[waitingRecipeSOIndex];

        _WaitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(DeliveryCounter sender, PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _WaitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _WaitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }

                    } // end foreach plate item

                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentMatchesRecipe = false;
                    }

                } // end foreach recipe item

                if (plateContentMatchesRecipe)
                {
                    // Player delivered a waiting recipe!
                    DeliverCorrectRecipeServerRpc(i, sender.transform.position);
                    return;
                }
            }

        } // end for i

        // No matches found!
        // The player did not deliver a correct recipe.
        DeliverIncorrectRecipeServerRpc(sender.transform.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc(Vector3 deliveryCounterPosition)
    {
        DeliverIncorrectRecipeClientRpc(deliveryCounterPosition);
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc(Vector3 deliveryCounterPosition)
    {

        OnRecipeFailed?.Invoke(this, new RecipeDeliveredEventHandler
        {
            DeliveryCounterPosition = deliveryCounterPosition
        });
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOIndex, Vector3 deliveryCounterPosition)    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOIndex, deliveryCounterPosition);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOIndex, Vector3 deliveryCounterPosition)
    {
        _SuccessfulRecipeAmount++;

        _WaitingRecipeSOList.RemoveAt(waitingRecipeSOIndex);

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);


        OnRecipeSuccess?.Invoke(this, new RecipeDeliveredEventHandler
        {
            DeliveryCounterPosition = deliveryCounterPosition
        }); 
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return _WaitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return _SuccessfulRecipeAmount;
    }
}
