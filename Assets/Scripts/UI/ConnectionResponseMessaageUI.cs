using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;


public class ConnectionResponseMessaageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _MessageText;
    [SerializeField] private Button _CloseButton;


    private void Awake()
    {
        _CloseButton.onClick.AddListener(OnCloseClicked);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedtoJoinGame += KitchenGameMultiplayer_OnFailedtoJoinGame;

        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedtoJoinGame -= KitchenGameMultiplayer_OnFailedtoJoinGame;
    }

    private void KitchenGameMultiplayer_OnFailedtoJoinGame(object sender, System.EventArgs e)
    {
        Show();

        _MessageText.text = NetworkManager.Singleton.DisconnectReason;

        if (_MessageText.text == "")
            _MessageText.text = "Failed to connect (timed out).";
    }

    private void OnCloseClicked()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
