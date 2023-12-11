using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _KitchenObjectSO;


    private IKitchenObjectParent _KitchenObjectParent;



    public KitchenObjectSO GetKitchenObjectSO()
    { 
        return _KitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this._KitchenObjectParent != null)
            this._KitchenObjectParent.ClearKitchenObject();


        this._KitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");

        kitchenObjectParent.SetKitchenObject(this);


        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _KitchenObjectParent;
    }

    public void DestroySelf()
    {
        _KitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
