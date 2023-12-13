using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _PlateKitchenObject;
    [SerializeField] private Transform _IconTemplate;



    private void Awake()
    {
        _IconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _PlateKitchenObject.OnIngredientAdded += _PlateKitchenObject_OnIngredientAdded;
    }

    private void _PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == _IconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in _PlateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(_IconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
