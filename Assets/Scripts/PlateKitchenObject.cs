using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> _ValidKitchenObjectSOList;
    private List<KitchenObjectSO> _KitchenObjectSOList;




    private void Awake()
    {
        _KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_ValidKitchenObjectSOList.Contains(kitchenObjectSO)) 
        {
            // Not a valid ingredient
            return false;
        }


        if (_KitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            _KitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
}
