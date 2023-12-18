using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;


public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _PlatesCounter;
    [SerializeField] private Transform _CounterTopPoint;
    [SerializeField] private Transform _PlateVisualPrefab;

    private List<GameObject> _PlateVisualGameObjectsList;



    private void Awake()
    {
        _PlateVisualGameObjectsList = new List<GameObject>();
    }
    private void Start()
    {
        _PlatesCounter.OnPlateSpawned += _PlatesCounter_OnPlateSpawned;
        _PlatesCounter.OnPlateRemoved += _PlatesCounter_OnPlateRemoved;
    }

    private void _PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = _PlateVisualGameObjectsList[_PlateVisualGameObjectsList.Count - 1];
        _PlateVisualGameObjectsList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void _PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(_PlateVisualPrefab, _CounterTopPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _PlateVisualGameObjectsList.Count, 0);

        _PlateVisualGameObjectsList.Add(plateVisualTransform.gameObject);
    }
}
