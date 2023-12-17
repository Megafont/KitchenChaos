using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class GameStartCoutndownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "CountdownToStartPopup";


    [SerializeField] private TextMeshProUGUI _CountdownText;


    private Animator _Animator;
    private int _PreviousCountdownNumber;



    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += Instance_OnStateChanged;

        Hide();
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        _CountdownText.text = countdownNumber.ToString();

        if (_PreviousCountdownNumber != countdownNumber)
        {
            if (countdownNumber > 0)
            {
                _PreviousCountdownNumber = countdownNumber;
                _Animator.SetTrigger(NUMBER_POPUP);
            }

            SoundManager.Instance.PlayCountdownSound();
        }
                                
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
