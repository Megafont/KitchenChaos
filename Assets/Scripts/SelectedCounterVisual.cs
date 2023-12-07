using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter _ClearCounter;
    [SerializeField] private GameObject _VisualGameObject;



    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == _ClearCounter)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        _VisualGameObject.SetActive(true);
    }

    private void Hide()
    {
        _VisualGameObject.SetActive(false);
    }
}
