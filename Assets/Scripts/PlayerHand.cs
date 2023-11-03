using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardInHand
{
    public CardDescriptor CardDescriptor;
    public GameObject SpawnedObject;
    public Vector3 BasePosition;
    public float BaseRotation;
}

public class ActiveCard
{
    public enum State
    {
        None,
        CursorHover,
        CursorFollow
    }

    public CardInHand Card;
    public State CurrentState;
}


public class PlayerHand : MonoBehaviour
{
    private readonly List<CardInHand> _cardsInHand = new();
    private Camera _camera;

    private readonly ActiveCard _activeCard = new();

    [SerializeField]
    private float _lerpTime = 0.1f;

    private void Awake()
    {
        _camera = LevelManager.Instance.MainCamera;

        var fireballCard = GameManager.Instance.GetCardDescriptor("card_fireball");
        AddCard(fireballCard);
        AddCard(fireballCard);
        AddCard(fireballCard);
        AddCard(fireballCard);
        AddCard(fireballCard);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            var fireballCard = GameManager.Instance.GetCardDescriptor("card_fireball");
            AddCard(fireballCard);
        }

        UpdateCardState();
        ApplyCardStateOnTransform();
    }

    public void AddCard(CardDescriptor card)
    {
        var cardInHand = new CardInHand();
        GameObject spawnedObject = Instantiate(card.Prefab, transform);
        spawnedObject.transform.localPosition = Vector3.zero;

        cardInHand.CardDescriptor = card;
        cardInHand.SpawnedObject = spawnedObject;

        _cardsInHand.Add(cardInHand);
        UpdateCardsLayout();
    }

    private void UpdateCardState()
    {
        if (Input.GetMouseButtonUp(0) && _activeCard.CurrentState == ActiveCard.State.CursorFollow)
        {
            _activeCard.CurrentState = ActiveCard.State.None;
        }
        else if (_activeCard.CurrentState != ActiveCard.State.CursorFollow)
        {
            _activeCard.Card = null;
            _activeCard.CurrentState = ActiveCard.State.None;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
            {
                if (hit.collider.gameObject.CompareTag("Card"))
                {
                    int cardIndex = int.Parse(hit.collider.gameObject.name);
                    _activeCard.Card = _cardsInHand[cardIndex];
                    _activeCard.CurrentState = Input.GetMouseButtonDown(0) ?
                        ActiveCard.State.CursorFollow :
                        ActiveCard.State.CursorHover;
                }
            }
        }
    }

    private void ApplyCardStateOnTransform()
    {
        foreach (CardInHand card in _cardsInHand)
        {
            Transform spawnedTransform = card.SpawnedObject.transform;
            if (_activeCard.Card == card && _activeCard.CurrentState != ActiveCard.State.None)
            {
                if (_activeCard.CurrentState == ActiveCard.State.CursorHover)
                {
                    spawnedTransform.localPosition = Vector3.Slerp(
                        spawnedTransform.localPosition, card.BasePosition - spawnedTransform.forward * 0.5f, _lerpTime);
                }
                else if (_activeCard.CurrentState == ActiveCard.State.CursorFollow)
                {
                    Vector3 worldMousePos = _camera.ScreenToWorldPoint(new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        5.0f
                    ));

                    Vector3 localMousePos = transform.InverseTransformPoint(worldMousePos);
                    spawnedTransform.localPosition = new Vector3(
                        localMousePos.x,
                        localMousePos.y,
                        spawnedTransform.localPosition.z
                    );

                    spawnedTransform.localEulerAngles = new Vector3(
                        0, 0, Mathf.LerpAngle(spawnedTransform.localEulerAngles.z, 0.0f, _lerpTime));
                }
            }
            else
            {
                spawnedTransform.localPosition = Vector3.Slerp(
                    spawnedTransform.localPosition, card.BasePosition, _lerpTime);
                spawnedTransform.localEulerAngles = new Vector3(
                    0, 0, Mathf.LerpAngle(spawnedTransform.localEulerAngles.z, card.BaseRotation, _lerpTime));
            }
        }
    }

    private void UpdateCardsLayout()
    {
        float startPosX = -0.5f * (_cardsInHand.Count - 1);
        float posX = startPosX;
        float rotZ = 1.5f * (_cardsInHand.Count - 1);
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            var card = _cardsInHand[i];
            float posY = Mathf.Cos(posX * (0.3f * Mathf.PI / Mathf.Abs(startPosX == 0 ? 0.001f : startPosX))) - 1.0f;
            card.BasePosition = new Vector3(posX, posY, 0);
            card.BaseRotation = rotZ;
            card.SpawnedObject.name = i.ToString();
            posX += 1.0f;
            rotZ -= 3.0f;
        }
    }
}
