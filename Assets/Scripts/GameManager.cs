using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardDescriptor
{
    public string Id;
    public string Name;
    public GameObject Prefab;
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private CardDescriptor[] _cardDescriptors;

    private Dictionary<string, CardDescriptor> _cardDescriptorsMap = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var cardDescriptor in _cardDescriptors)
        {
            _cardDescriptorsMap[cardDescriptor.Id] = cardDescriptor;
        }
    }

    public CardDescriptor GetCardDescriptor(string id)
    {
        return _cardDescriptorsMap.TryGetValue(id, out CardDescriptor value) ? value : null;
    }
}
