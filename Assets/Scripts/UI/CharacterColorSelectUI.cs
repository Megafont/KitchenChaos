using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class runs the character color selection UI.
/// </summary>
/// <remarks>
/// NOTE: The original version of the character color selection UI is still present in the CharacterSelectScene.
///       That UI did not have a script on it, so this script is just for my version, which supports adding more
///       colors without manually adding more buttons to the UI for them. Colors are added in the LobbyScene,
///       on the KitchenGameMultiplayer GameObject.
/// </remarks>
public class CharacterColorSelectUI : MonoBehaviour
{
    [SerializeField] private Button _ColorButtonPrefab;
    [SerializeField] private Transform _ColorButtonsParent;



    private void Start()
    {
        CreateColorButtons();
    }

    private void CreateColorButtons()
    {
        int colorCount = KitchenGameMultiplayer.Instance.GetPlayerColorsCount();
        for (int i = 0; i < colorCount; i++) 
        { 
            Color color = KitchenGameMultiplayer.Instance.GetPlayerColor(i);

            Button newButton = Instantiate(_ColorButtonPrefab, _ColorButtonsParent);
            newButton.GetComponent<CharacterColorSelectButtonUI>().SetColorIndex(i);
        }
    }
}
