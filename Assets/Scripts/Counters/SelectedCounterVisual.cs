using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _BaseCounter;
    [SerializeField] private GameObject[] _VisualGameObjectArray;



    private void Start()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            // We do -= first here, so that no matter how many times this event method is called,
            // we will only have one listener setup.
            Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == _BaseCounter)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in _VisualGameObjectArray)
            visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in _VisualGameObjectArray)
            visualGameObject.SetActive(false);
    }

}
