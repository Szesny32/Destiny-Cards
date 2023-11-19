using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;


public class CardDescriptor
{
    public CardType Type;
    public CardCategory Category;
    public string Name;
    public GameObject Prefab;
    public GameObject EffectPrefab;
    public GameObject SecondEffectPrefab;
    public GameObject HighlightEffect;
    public GameObject HoldEffect;
    public ICardEffectHandler CardEffectHandler;

}

public enum CardType
{
    Pull,
    Fireball,
    ResizeUp,
    ResizeDown,
    SpectralVision
};

public enum CardCategory
{
   Target,
   World
};



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
    private readonly Dictionary<CardType, CardDescriptor> _cardDescriptorsMap = new();

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
        _cardDescriptorsMap[CardType.Pull] = new CardDescriptor
        {
            Type = CardType.Pull,
            Category = CardCategory.Target,
            Name = "Pull",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new PullEffectHandler(),
        };

        _cardDescriptorsMap[CardType.Fireball] = new CardDescriptor
        {
            Type = CardType.Fireball,
            Category = CardCategory.Target,
            Name = "Fireball",
            Prefab = GetPrefab("card_fireball"),
            EffectPrefab = GetPrefab("effect_holyhit"),
            SecondEffectPrefab = GetPrefab("effect_fire_meteor"),
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new FireballEffectHandler(),
        };

        _cardDescriptorsMap[CardType.ResizeUp] = new CardDescriptor
        {
            Type = CardType.ResizeUp,
            Category = CardCategory.Target,
            Name = "ResizeUp",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new ResizeUpEffectHandler(),
        };

        _cardDescriptorsMap[CardType.ResizeDown] = new CardDescriptor
        {
            Type = CardType.ResizeDown,
            Category = CardCategory.Target,
            Name = "ResizeDown",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new ResizeDownEffectHandler(),
        };

        _cardDescriptorsMap[CardType.SpectralVision] = new CardDescriptor
        {
            Type = CardType.SpectralVision,
            Category = CardCategory.World,
            Name = "SpectralVision",
            Prefab = GetPrefab("card_pull"),
            EffectPrefab = GetPrefab("effect_magic_cricle"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new SpectralVisionEffectHandler(),
        };

        

    }

    public CardDescriptor GetCardDescriptor(CardType type)
    {
        return _cardDescriptorsMap.TryGetValue(type, out CardDescriptor value) ? value : null;
    }

    public GameObject GetPrefab(string id)
    {
        return _prefabsMap.TryGetValue(id, out GameObject value) ? value : null;
    }
}
