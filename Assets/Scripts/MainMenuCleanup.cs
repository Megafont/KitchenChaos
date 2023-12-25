using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;


public class MainMenuCleanup : MonoBehaviour
{
    private void Awake()
    {
        // Cleanup the singletons that are still around if we returned to the main
        // menu from the game.

        if (NetworkManager.Singleton != null)
            Destroy(NetworkManager.Singleton.gameObject);

        if (KitchenGameMultiplayer.Instance != null)
            Destroy(KitchenGameMultiplayer.Instance.gameObject);

        if (KitchenGameLobby.Instance != null)
            Destroy(KitchenGameLobby.Instance.gameObject);
    }
}
