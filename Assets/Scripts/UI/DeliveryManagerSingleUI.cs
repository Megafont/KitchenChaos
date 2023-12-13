using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _RecipeNameText;
    [SerializeField] Transform _IconContainer;
    [SerializeField] Transform _IconTemplate;



    private void Awake()
    {
        _IconTemplate.gameObject.SetActive(false);

    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        _RecipeNameText.text = recipeSO.RecipeName;

        foreach (Transform child in _IconContainer)
        {
            if (child == _IconTemplate)
                continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(_IconTemplate, _IconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
        }
    }
}
