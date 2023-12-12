using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public float FryingTimerMax; // Frying time.
}
