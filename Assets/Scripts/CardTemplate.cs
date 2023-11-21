using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardTemplate : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _frontRenderer;

    [SerializeField]
    private TMP_Text _cardNameTextMesh;

    public void SetFromDescriptor(CardDescriptor cardDescriptor)
    {
        _frontRenderer.material.mainTexture = cardDescriptor.FrontTexture;
        _cardNameTextMesh.text = cardDescriptor.Name;
    }
}
