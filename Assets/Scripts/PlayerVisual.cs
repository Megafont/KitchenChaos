using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _HeadMeshRenderer;
    [SerializeField] private MeshRenderer _BodyMeshRenderer;


    private Material _Material;



    private void Awake()
    {
        // Clone the original material, because each player has to have their own material
        // so they can be different colors.
        _Material = new Material(_HeadMeshRenderer.material);

        _HeadMeshRenderer.material = _Material;
        _BodyMeshRenderer.material = _Material;
    }

    public void SetPlayerColor(Color color)
    {
        _Material.color = color;
    }
}
