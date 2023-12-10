using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _KitchenObjectSO;



    public override void Interact(Player player)
    {
        Transform kitchenObjectTransform = Instantiate(_KitchenObjectSO.Prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

    }


}
