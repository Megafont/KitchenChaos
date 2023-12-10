using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _KitchenObjectSO;
    [SerializeField] private Transform _CounterTopPoint;



    private KitchenObject _KitchenObject;



    public void Interact(Player player)
    {
        if (_KitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(_KitchenObjectSO.Prefab, _CounterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            // Give the object to the player.
            _KitchenObject.SetKitchenObjectParent(player);
        }

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _CounterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _KitchenObject = kitchenObject;
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
