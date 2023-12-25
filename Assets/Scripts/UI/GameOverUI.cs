using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;


public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _RecipesDeliveredText;
    [SerializeField] private Button _PlayAgainButton;



    private void Awake()
    {
        _PlayAgainButton.onClick.AddListener(OnPlayAgainClicked);

        _PlayAgainButton.Select();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void OnPlayAgainClicked()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Loader.Scenes.MainMenuScene);
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();

            _RecipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);

        _PlayAgainButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
