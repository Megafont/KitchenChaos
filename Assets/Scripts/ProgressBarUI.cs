using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter _CuttingCounter;
    [SerializeField] private Image _BarImage;


    private void Start()
    {
        _CuttingCounter.OnProgressChanged += _CuttingCounter_OnProgressChanged;

        _BarImage.fillAmount = 0f;

        // This must be called AFTER we subscribe to the OnProgressChanged event,
        // because Hide() disables this GameObject.
        Hide();
    }

    private void _CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        _BarImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0f || e.ProgressNormalized == 1f)
            Hide();
        else
            Show();
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
