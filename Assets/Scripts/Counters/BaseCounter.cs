using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }


    [SerializeField] private Transform _CounterTopPoint;


    private KitchenObject _KitchenObject;



    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player)
    {
        // This debug code is commented out now since not all counters override this function.
        //Debug.LogError("BaseCounter.InteractAlternate()");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _CounterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _KitchenObject = kitchenObject;

        if (kitchenObject != null) 
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return _KitchenObject;
    }

    public void ClearKitchenObject()
    {
        _KitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _KitchenObject != null;
    }

}
