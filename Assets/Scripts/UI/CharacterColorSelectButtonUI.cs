using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterColorSelectButtonUI : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    [SerializeField] private int _ColorIndex;
    [SerializeField] private Image _Image;
    [SerializeField] private GameObject _SelectedGameObject;

    private Button _Button;
    private Image _SelectionOutlineImage;



    private void Awake()
    {
        _Button = GetComponent<Button>();

        _Button.onClick.AddListener(OnClicked);

        _SelectionOutlineImage = _SelectedGameObject.GetComponent<Image>();
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
        _SelectionOutlineImage.color = Color.white;

        if (KitchenGameMultiplayer.Instance.GetLocalPlayerData().ColorIndex == _ColorIndex)
            _SelectedGameObject.SetActive(true);
        else
            _SelectedGameObject.SetActive(false);
    }

    private void UpdateIsHighlighted()
    {
        if (IsHighlighted)
        {
            _SelectedGameObject.SetActive(true);
            _SelectionOutlineImage.color = Color.yellow;
        }
        else
        {
            if (IsSelected)
            {
                UpdateIsSelected();
            }
            else
            {
                _SelectionOutlineImage.color = Color.white;
                _SelectedGameObject.SetActive(false);
            }
        }
    }


    public void SetColorIndex(int colorIndex)
    {
        _ColorIndex = colorIndex;

        UpdateImageColor();
        UpdateIsSelected();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        IsHighlighted = false;
        
        UpdateIsHighlighted();
    }

    public void OnSelect(BaseEventData eventData)
    {
        IsHighlighted = true;

        UpdateIsHighlighted();
    }



    public bool IsHighlighted { get; private set; }
    public bool IsSelected { get { return KitchenGameMultiplayer.Instance.GetLocalPlayerData().ColorIndex == _ColorIndex; } }
}
