using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Unity.Netcode;
using UnityEngine;


public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _KitchenObjectSO;


    private IKitchenObjectParent _KitchenObjectParent;
    private FollowTransform _FollowTransform;



    protected virtual void Awake()
    {
        _FollowTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    { 
        return _KitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (kitchenObjectParent == null)
        {
            Debug.LogError("Failed to get the kitchen object parent from the reference!");
            return;
        }


        if (_KitchenObjectParent != null)
            _KitchenObjectParent.ClearKitchenObject();


        _KitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");

        kitchenObjectParent.SetKitchenObject(this);


        _FollowTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());

    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _KitchenObjectParent;
    }

    /// <summary>
    /// Destroys this KitchenObject.
    /// </summary>
    /// <remarks>
    /// This function is only called in KitchenGameMultiplayer.DestroyKitchenObjectServerRpc().
    /// That's because only the server should destroy the object.
    /// </remarks>
    public void DestroySelf()
    {

        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        _KitchenObjectParent.ClearKitchenObject();
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }

    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
}
