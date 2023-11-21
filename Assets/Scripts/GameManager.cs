using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;


public class CardDescriptor
{
    public CardType Type;
    public CardCategory Category;
    public string Name;
    public Texture FrontTexture;
    public GameObject EffectPrefab;
    public GameObject SecondEffectPrefab;
    public GameObject HighlightEffect;
    public GameObject HoldEffect;
    public CardEffectHandler CardEffectHandler;

}

public enum CardType
{
    None,
    Pull,
    Fireball,
    ResizeUp,
    ResizeDown,
    SpectralVision,
    Open
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

[System.Serializable]
public class TextureDescriptor
{
    public string Id;
    public Texture Texture;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject _cardTemplatePrefab;

    [SerializeField]
    private PrefabDescriptor[] _prefabs;

    [SerializeField]
    private TextureDescriptor[] _textures;

    [SerializeField]
    private string[] _levelSceneNames;

    public GameObject CardTemplatePrefab => _cardTemplatePrefab;

    private int _currentLevelIndex = 0;

    private readonly Dictionary<string, GameObject> _prefabsMap = new();
    private readonly Dictionary<string, Texture> _texturesMap = new();
    private readonly Dictionary<CardType, CardDescriptor> _cardDescriptorsMap = new();

    private List<CardDescriptor> _savedCardsInHand = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (PrefabDescriptor prefab in _prefabs)
        {
            _prefabsMap[prefab.Id] = prefab.Prefab;
        }

        foreach (TextureDescriptor texture in _textures)
        {
            _texturesMap[texture.Id] = texture.Texture;
        }

        PrepareCardDescriptors();
    }

    public void NextLevel()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex < _levelSceneNames.Length)
        {
            string sceneName = _levelSceneNames[_currentLevelIndex];
            _savedCardsInHand = new List<CardDescriptor>();
            foreach (var cardInHand in LevelManager.Instance.PlayerHand.CardsInHand)
            {
                _savedCardsInHand.Add(cardInHand.CardDescriptor);
            }

            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    private void PrepareCardDescriptors()
    {
        _cardDescriptorsMap[CardType.Pull] = new CardDescriptor
        {
            Type = CardType.Pull,
            Category = CardCategory.Target,
            Name = "Pull",
            FrontTexture = GetTexture("tx_pull"),
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
            FrontTexture = GetTexture("tx_fireball"),
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
            Name = "Resize Up",
            FrontTexture = GetTexture("tx_resize_up"),
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
            Name = "Resize Down",
            FrontTexture = GetTexture("tx_resize_down"),
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
            Name = "Spectral Vision",
            FrontTexture = GetTexture("tx_spectral_vision"),
            EffectPrefab = GetPrefab("effect_magic_cricle"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new SpectralVisionEffectHandler(),
        };

        _cardDescriptorsMap[CardType.Open] = new CardDescriptor
        {
            Type = CardType.Open,
            Category = CardCategory.Target,
            Name = "Open",
            FrontTexture = GetTexture("tx_open"),
            EffectPrefab = GetPrefab("effect_greenhit"),
            SecondEffectPrefab = null,
            HighlightEffect = GetPrefab("effect_portal_blue"),
            HoldEffect = GetPrefab("effect_magic_cricle"),
            CardEffectHandler = new OpenEffectHandler(),
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

    public Texture GetTexture(string id)
    {
        return _texturesMap.TryGetValue(id, out Texture value) ? value : null;
    }

    public List<CardDescriptor> SavedCardsInHand => _savedCardsInHand;
}
