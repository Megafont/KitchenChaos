using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject GameObject;
    }



    [SerializeField] private PlateKitchenObject _PlateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _KitchenObjectSOGameObjectList;



    private void Start()
    {
        _PlateKitchenObject.OnIngredientAdded += _PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in _KitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.GameObject.SetActive(false);
        }
    }

    private void _PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in _KitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.KitchenObjectSO == e.KitchenObjectSO)
            {
                kitchenObjectSOGameObject.GameObject.SetActive(true);
            }
        }
    }
}
