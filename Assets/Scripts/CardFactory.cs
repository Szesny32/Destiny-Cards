using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory
{
    private readonly GameObject _cardPrefab;

    public CardFactory(GameObject cardPrefab)
    {
        _cardPrefab = cardPrefab;
    }

    public CardInHand CreateCardInHand(Transform parentTransform, CardDescriptor cardDescriptor)
    {
        var cardInHand = new CardInHand();
        GameObject spawnedObject = Object.Instantiate(_cardPrefab, parentTransform);
        spawnedObject.transform.localPosition = new Vector3(1, 3, 0);

        cardInHand.CardDescriptor = cardDescriptor;
        cardInHand.SpawnedObject = spawnedObject;

        cardInHand.FlowOffset = new Vector3(
            Random.Range(0f, 360f) * Mathf.Deg2Rad,
            Random.Range(0f, 360f) * Mathf.Deg2Rad,
            Random.Range(0f, 360f) * Mathf.Deg2Rad
        );

        var cardTemplate = spawnedObject.GetComponent<CardTemplate>();
        cardTemplate.SetFromDescriptor(cardDescriptor);

        return cardInHand;
    }

    public GameObject CreateCollectableCard(Transform parentTransform, CardDescriptor cardDescriptor)
    {
        GameObject spawnedObject = Object.Instantiate(_cardPrefab, parentTransform);
        spawnedObject.transform.localPosition = Vector3.zero;

        var cardTemplate = spawnedObject.GetComponent<CardTemplate>();
        cardTemplate.SetFromDescriptor(cardDescriptor);
        return spawnedObject;
    }
}