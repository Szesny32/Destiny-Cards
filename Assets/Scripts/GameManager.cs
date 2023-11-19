using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;


public class CardDescriptor
{
    public string Id;
    public string Name;
    public GameObject Prefab;
    public GameObject EffectPrefab;
    public GameObject HighlightEffect;
    public GameObject HoldEffect;
    public ICardEffectHandler CardEffectHandler;

}

[System.Serializable]
public class PrefabDescriptor
{
    public string Id;
    public GameObject Prefab;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private PrefabDescriptor[] _prefabs;

    private readonly Dictionary<string, GameObject> _prefabsMap = new();
    private readonly Dictionary<string, CardDescriptor> _cardDescriptorsMap = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (PrefabDescriptor prefab in _prefabs)
        {
            _prefabsMap[prefab.Id] = prefab.Prefab;
        }
        PrepareCardDescriptors();
    }

    private void PrepareCardDescriptors()
    {
        _cardDescriptorsMap["card_pull"] = new CardDescriptor
        {
            Id = "card_pull",
            Name = "Pull",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new PullEffectHandler(),
        };

        _cardDescriptorsMap["card_fireball"] = new CardDescriptor
        {
            Id = "card_fireball",
            Name = "Fireball",
            Prefab = GetPrefab("card_fireball"),
            EffectPrefab = GetPrefab("effect_holyhit"),
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new FireballEffectHandler(),
        };

        _cardDescriptorsMap["card_resizeUp"] = new CardDescriptor
        {
            Id = "card_resizeUp",
            Name = "ResizeUp",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new ResizeUpEffectHandler(),
        };

            _cardDescriptorsMap["card_resizeDown"] = new CardDescriptor
        {
            Id = "card_resizeDown",
            Name = "ResizeDown",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new ResizeDownEffectHandler(),
        };

    }

    public CardDescriptor GetCardDescriptor(string id)
    {
        return _cardDescriptorsMap.TryGetValue(id, out CardDescriptor value) ? value : null;
    }

    public GameObject GetPrefab(string id)
    {
        return _prefabsMap.TryGetValue(id, out GameObject value) ? value : null;
    }
}
