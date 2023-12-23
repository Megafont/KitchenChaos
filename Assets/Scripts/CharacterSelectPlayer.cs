using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int _PlayerIndex;
    [SerializeField] private GameObject _ReadyText;
    [SerializeField] private PlayerVisual _PlayerVisual;
    [SerializeField] private Button _KickButton;



    private void Awake()
    {
        _KickButton.onClick.AddListener(OnKickClicked);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        // Show the kick buttons only if this is the host.
        _KickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        
        UpdatePlayer();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
    }

    private void OnKickClicked()
    {
        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_PlayerIndex);
        KitchenGameMultiplayer.Instance.KickPlayer(playerData.ClientId);
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void KitchenGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (KitchenGameMultiplayer.Instance.IsPlayerConnected(_PlayerIndex))
        {
            Show();

            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_PlayerIndex);
            _ReadyText.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.ClientId));

            _PlayerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.ColorIndex));
        }
        else
        {
            Hide();
        }
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
