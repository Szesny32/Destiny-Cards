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
    public ICardEffectHandler CardEffectHandler;
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private readonly Dictionary<string, CardDescriptor> _cardDescriptorsMap = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PrepareCardDescriptors();
    }

    private void PrepareCardDescriptors()
    {
        _cardDescriptorsMap["card_pull"] = new CardDescriptor
        {
            Id = "card_pull",
            Name = "Pull",
            Prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Cards/Card_Fireball.prefab"),
            EffectPrefab = PrefabUtility.LoadPrefabContents("Assets/Hovl Studio/Magic effects pack/Prefabs/Hits and explosions/Green hit.prefab"),
            CardEffectHandler = new PullEffectHandler()
        };

        _cardDescriptorsMap["card_fireball"] = new CardDescriptor
        {
            Id = "card_fireball",
            Name = "Fireball",
            Prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Cards/Card_Fireball.prefab"),
            EffectPrefab = PrefabUtility.LoadPrefabContents("Assets/Hovl Studio/Magic effects pack/Prefabs/Hits and explosions/Holy hit.prefab"),
            CardEffectHandler = new FireballEffectHandler()
        };
    }

    public CardDescriptor GetCardDescriptor(string id)
    {
        return _cardDescriptorsMap.TryGetValue(id, out CardDescriptor value) ? value : null;
    }
}
