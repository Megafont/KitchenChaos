using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StoveBurningWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _StoveCounter;


    private void Start()
    {
        _StoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = _StoveCounter.IsFried() && e.ProgressNormalized >= StoveCounter.STOVE_BURNING_WARNING_TIME;

        if (show)
            Show();
        else
            Hide();
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
