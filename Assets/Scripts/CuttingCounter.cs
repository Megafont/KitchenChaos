using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] _CuttingRecipeSOArray;



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
                    // Player is carrying something that can be cut.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a KitchObject here AND it can be cut
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in _CuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.Input == inputKitchenObjectSO)
                return true;
        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in _CuttingRecipeSOArray) 
        {
            if (cuttingRecipeSO.Input == inputKitchenObjectSO)
                return cuttingRecipeSO.Output;
        }

        return null;
    }
}
