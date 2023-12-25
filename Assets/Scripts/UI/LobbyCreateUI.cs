using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button _CloseButton;
    [SerializeField] private Button _CreatePrivateButton;
    [SerializeField] private Button _CreatePublicButton;
    [SerializeField] private TMP_InputField _LobbyNameInputField;


    private void Awake()
    {
        _CloseButton.onClick.AddListener(OnCloseClicked);
        _CreatePrivateButton.onClick.AddListener(OnCreatePrivateClicked);
        _CreatePublicButton.onClick.AddListener(OnCreatePublicClicked);

        _CreatePublicButton.Select();
    }

    private void Start()
    {
        Hide();
    }

    private void OnCloseClicked()
    {
        Hide();
    }

    private void OnCreatePrivateClicked()
    {
        if (string.IsNullOrWhiteSpace(_LobbyNameInputField.text))
            return;

        KitchenGameLobby.Instance.CreateLobby(_LobbyNameInputField.text, true);
    }

    private void OnCreatePublicClicked()
    {
        if (string.IsNullOrWhiteSpace(_LobbyNameInputField.text))
            return;

        KitchenGameLobby.Instance.CreateLobby(_LobbyNameInputField.text, false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
