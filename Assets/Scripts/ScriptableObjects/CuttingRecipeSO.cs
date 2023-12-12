using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public int CuttingProgressMax; // How many chops it takes to cut the input item.
}
