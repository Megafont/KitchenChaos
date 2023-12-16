using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;



    [SerializeField] private KitchenObjectSO _KitchenObjectSO;



    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            KitchenObject.SpawnKitchenObject(_KitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
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
                    if (plateKitchenObject.TryAddIngredient(_KitchenObjectSO))
                    {                       
                        // The above call to TryAddIngredient() adds the ingredient if possible,
                        // so there is nothing to do here.
                    }
                }
                else
                {
                    // Player is not holding a plate, but something else
                }
            }
        }

    }


}
