using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _CreateGameButton;
    [SerializeField] private Button _JoinGameButton;


    private void Awake()
    {
        _CreateGameButton.onClick.AddListener(OnCreateGameClicked);
        _JoinGameButton.onClick.AddListener(OnJoinButtonClicked);

        _CreateGameButton.Select();
    }

    private void OnCreateGameClicked()
    {
        KitchenGameMultiplayer.Instance.StartHost();
        Loader.LoadSceneMultiplayer(Loader.Scenes.CharacterSelectScene);
    }

    private void OnJoinButtonClicked()
    {
        KitchenGameMultiplayer.Instance.StartClient();
    }

}
