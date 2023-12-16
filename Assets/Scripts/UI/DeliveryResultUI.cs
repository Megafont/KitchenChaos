using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";


    [SerializeField] private Image _BackgroundImage;
    [SerializeField] private Image _IconImage;
    [SerializeField] private TextMeshProUGUI _MessageText;

    [Space(10)]
    [SerializeField] private Color _SuccessColor;
    [SerializeField] private Sprite _SuccessSprite;

    [Space(10)]
    [SerializeField] private Color _FailedColor;
    [SerializeField] private Sprite _FailedSprite;


    private Animator _Animator;



    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        _BackgroundImage.color = _FailedColor;
        _IconImage.sprite = _FailedSprite;
        _MessageText.text = "DELIVERY\nFAILED!";

        _Animator.SetTrigger(POPUP);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        _BackgroundImage.color = _SuccessColor;
        _IconImage.sprite = _SuccessSprite;
        _MessageText.text = "DELIVERY\nSUCCESS!";

        _Animator.SetTrigger(POPUP);
    }
}
