using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private CardType[] _cardsInside;

    [SerializeField]
    private GameObject _collectableCardPrefab;

    public bool IsOpened { get; private set; } = false;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (IsOpened)
        {
            return;
        }

        _animator.SetTrigger("open");
        IsOpened = true;

        StartCoroutine(ThrowCardsCoroutine());
    }

    private IEnumerator ThrowCardsCoroutine()
    {
        foreach (var card in _cardsInside)
        {
            yield return new WaitForSeconds(0.8f);
            GameObject spawnedCollectable = Instantiate(_collectableCardPrefab);
            spawnedCollectable.transform.position = transform.position + new Vector3(0, 2, 0);

            CollectableCard collectableCard = spawnedCollectable.GetComponent<CollectableCard>();
            collectableCard.SpawnCard(card);

            Rigidbody spawnedRigidbody = spawnedCollectable.GetComponent<Rigidbody>();

            float random = Random.Range(-20, 20);
            float angle = (spawnedCollectable.transform.eulerAngles.y + random) * Mathf.Deg2Rad;
            spawnedRigidbody.velocity = new Vector3(Mathf.Cos(angle) * 4, 8, Mathf.Sin(angle) * 4);
        }
    }
}
