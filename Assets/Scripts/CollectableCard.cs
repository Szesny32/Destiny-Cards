using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCard : MonoBehaviour
{
    [SerializeField]
    private CardType _cardType = CardType.None;

    private float _timer = 0;
    private GameObject _spawnedCardObject;
    private CardDescriptor _cardDescriptor;

    public void SpawnCard(CardType cardType)
    {
        var cardFactory = new CardFactory(GameManager.Instance.CardTemplatePrefab);
        _cardDescriptor = GameManager.Instance.GetCardDescriptor(cardType);
        _spawnedCardObject = cardFactory.CreateCollectableCard(transform, _cardDescriptor);
    }

    private void Awake()
    {
        if (_cardType != CardType.None)
        {
            SpawnCard(_cardType);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        float positionY = (Mathf.Sin(_timer * 3.5f) + 1.0f) * 0.25f;
        _spawnedCardObject.transform.localPosition = new Vector3(
            _spawnedCardObject.transform.localPosition.x, positionY, _spawnedCardObject.transform.localPosition.z);
        _spawnedCardObject.transform.localEulerAngles = new Vector3(
            _spawnedCardObject.transform.localEulerAngles.x, _timer * 120.0f, _spawnedCardObject.transform.localEulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        var magePlayer = other.gameObject.GetComponent<MagePlayerController>();
        if (magePlayer != null)
        {
            LevelManager.Instance.PlayerHand.AddCard(_cardDescriptor);
            Destroy(gameObject);
        }
    }
}
