using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO _RecipeListSO;
    [SerializeField] private float _SpawnRecipeTimerMax = 4f;
    [SerializeField] private int _WaitingRecipeMax = 4;


    private List<RecipeSO> _WaitingRecipeSOList;
    private float _SpawnRecipeTimer;


    private void Awake()
    {
        Instance = this;

        _WaitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        _SpawnRecipeTimer -= Time.deltaTime;
        if (_SpawnRecipeTimer <= 0f)
        {
            _SpawnRecipeTimer = _SpawnRecipeTimerMax;

            if (_WaitingRecipeSOList.Count < _WaitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = _RecipeListSO.RecipeSOList[Random.Range(0, _RecipeListSO.RecipeSOList.Count)];
                _WaitingRecipeSOList.Add(waitingRecipeSO);

                Debug.Log(waitingRecipeSO.RecipeName);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
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
                    Debug.Log("Player delivered a waiting recipe!");
                    _WaitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }

        } // end for i

        // No matches found!
        // The player did not deliver a correct recipe.
        Debug.Log("Player did not deliver a correct recipe!");
    }
}
