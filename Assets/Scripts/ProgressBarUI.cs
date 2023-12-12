using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _HasProgressGameObject;
    [SerializeField] private Image _BarImage;


    private IHasProgress _HasProgress;



    private void Start()
    {
        _HasProgress = _HasProgressGameObject.GetComponent<IHasProgress>();
        if (_HasProgress == null)
            Debug.LogError($"GameObject {_HasProgressGameObject} does not have a component that implements IHasProgress!");

        _HasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        _BarImage.fillAmount = 0f;

        // This must be called AFTER we subscribe to the OnProgressChanged event,
        // because Hide() disables this GameObject.
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
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
