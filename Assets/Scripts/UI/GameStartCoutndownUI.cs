using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEditor.Search;


public class GameStartCoutndownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _CountdownText;


    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += Instance_OnStateChanged;

        Hide();
    }

    private void Update()
    {
        _CountdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();   
    }

    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
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
