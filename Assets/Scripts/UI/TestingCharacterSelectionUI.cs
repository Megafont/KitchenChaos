using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class TestingCharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private Button _ReadyButton;


    private void Awake()
    {
        _ReadyButton.onClick.AddListener(OnReadyClicked);
    }

    private void OnReadyClicked()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }
}
