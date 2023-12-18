using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }



    [SerializeField] private List<KitchenObjectSO> _ValidKitchenObjectSOList;
    
    
    private List<KitchenObjectSO> _KitchenObjectSOList;




    protected override void Awake()
    {
        base.Awake();

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

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSO = kitchenObjectSO,
            });

            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return _KitchenObjectSOList;
    }
}
