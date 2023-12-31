using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button _PlayAgainButton;
    [SerializeField] private CharacterSelectUI _ParentUI;



    private void Awake()
    {
        _PlayAgainButton.onClick.AddListener(OnPlayAgainClicked);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        Hide();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            // Server is shutting down
            Show();
        }
    }

    private void OnPlayAgainClicked()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void Show()
    {
        gameObject.SetActive(true);

        _PlayAgainButton.Select();
    }

    private void Hide()
    {
        _ParentUI?.SelectDefaultButton();

        gameObject.SetActive(false);        
    }
}
