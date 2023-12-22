using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _StartAsClientButton;
    [SerializeField] private Button _StartAsHostButton;
    [SerializeField] private Button _StartAsServerButton;



    private void Awake()
    {
        _StartAsClientButton.onClick.AddListener(OnStartAsClientClicked);
        _StartAsHostButton.onClick.AddListener(OnStartAsHostClicked);
        _StartAsServerButton.onClick.AddListener(OnStartAsServerClicked);

        _StartAsClientButton.Select();
    }

    private void OnStartAsClientClicked()
    {
        Debug.Log("STARTED AS CLIENT");
        KitchenGameMultiplayer.Instance.StartClient();
        Hide();
    }

    private void OnStartAsHostClicked()
    {
        Debug.Log("STARTED AS HOST");
        KitchenGameMultiplayer.Instance.StartHost();
        Hide();
    }

    private void OnStartAsServerClicked()
    {
        Debug.Log("STARTED AS SERVER");
        KitchenGameMultiplayer.Instance.StartServer();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
