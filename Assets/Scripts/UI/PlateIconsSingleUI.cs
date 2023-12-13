using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class PlateIconsSingleUI : MonoBehaviour
{
    [SerializeField] private Image _Image;



    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        _Image.sprite = kitchenObjectSO.Sprite;
    }
}
