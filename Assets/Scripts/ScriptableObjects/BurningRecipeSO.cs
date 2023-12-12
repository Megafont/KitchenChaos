using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public float BurningTimerMax; // Burning time. This is a separate timer that starts when frying has finished.
}
