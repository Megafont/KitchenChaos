using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image _TimerImage;


    private void Update()
    {
        _TimerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
