using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _MainMenuButton;
    [SerializeField] private Button _ReadyButton;



    private void Awake()
    {
        _MainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _ReadyButton.onClick.AddListener(OnReadyClicked);
    }

    private void OnMainMenuClicked()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void OnReadyClicked()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }
}
