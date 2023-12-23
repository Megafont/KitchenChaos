using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class CharacterColorSelectButtonUI : MonoBehaviour
{
    [SerializeField] private int _ColorIndex;
    [SerializeField] private Image _Image;
    [SerializeField] private GameObject _SelectedGameObject;



    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
        
        UpdateImageColor();
        UpdateIsSelected();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
    }

    private void KitchenGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateIsSelected();
    }

    private void OnClicked()
    {
        KitchenGameMultiplayer.Instance.ChangePlayerColor(_ColorIndex);
    }

    private void UpdateImageColor()
    {
        _Image.color = KitchenGameMultiplayer.Instance.GetPlayerColor(_ColorIndex);
    }

    private void UpdateIsSelected()
    {
        if (KitchenGameMultiplayer.Instance.GetLocalPlayerData().ColorIndex == _ColorIndex)
            _SelectedGameObject.SetActive(true);
        else
            _SelectedGameObject.SetActive(false);
    }

    public void SetColorIndex(int colorIndex)
    {
        _ColorIndex = colorIndex;

        UpdateImageColor();
        UpdateIsSelected();
    }
}
