using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardInHand
{
    public CardDescriptor CardDescriptor;
    public GameObject SpawnedObject;
    public Vector3 BasePosition;
    public Vector3 flowOffset;
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

    [SerializeField]
    private float _amplitude = 0.04f;
    
    [SerializeField]
    private float cardOffset = 1.125f;

    [SerializeField]
    private float _minScale = 0.25f;

    [SerializeField]
    private float _maxScale = 0.75f;

    [SerializeField]
    private float _scaleFactor = 2f;
    
    [SerializeField]
    private float _cursorFollowOffset = 1f;

    [SerializeField]
    private Material _outline;

    private MagePlayerController _playerController;

    private void Awake(){
        _playerController = LevelManager.Instance.Player.GetComponent<MagePlayerController>();
        _camera = LevelManager.Instance.MainCamera;

        var pullCard = GameManager.Instance.GetCardDescriptor("card_pull");
        AddCard(pullCard);
        AddCard(pullCard);
        AddCard(pullCard);

        var fireballCard = GameManager.Instance.GetCardDescriptor("card_fireball");
        AddCard(fireballCard);
        AddCard(fireballCard);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.T))
        {
            var fireballCard = GameManager.Instance.GetCardDescriptor("card_fireball");
            AddCard(fireballCard);
        }

        UpdateCardState();
        ApplyCardStateOnTransform();
    }

    public void AddCard(CardDescriptor card){
        var cardInHand = new CardInHand();
        GameObject spawnedObject = Instantiate(card.Prefab, transform);
        spawnedObject.transform.localPosition = Vector3.zero;

        cardInHand.CardDescriptor = card;
        cardInHand.SpawnedObject = spawnedObject;

        cardInHand.flowOffset = new Vector3(  
            Random.Range(0f, 360f) * Mathf.Deg2Rad,
            Random.Range(0f, 360f) * Mathf.Deg2Rad,
            Random.Range(0f, 360f) * Mathf.Deg2Rad
        );

        _cardsInHand.Add(cardInHand);
        UpdateCardsLayout();
    }

    private void UpdateCardsLayout(){
        float startPosX = -0.5f *cardOffset* (_cardsInHand.Count - 1);
        float posX = startPosX;
        float rotZ = 1.5f * (_cardsInHand.Count - 1);
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            var card = _cardsInHand[i];
            float posY = Mathf.Cos(posX * (0.3f * Mathf.PI / Mathf.Abs(startPosX == 0 ? 0.001f : startPosX))) - 1.0f;
            card.BasePosition = new Vector3(posX, posY, 0);
            card.BaseRotation = rotZ;
            card.SpawnedObject.name = i.ToString();
            posX += cardOffset;
            rotZ -= 3.0f;
        }
    }



    //-----------------------------UpdateCardState-----------------------------
    private void UpdateCardState(){
        bool cardInUse = _activeCard.CurrentState == ActiveCard.State.CursorFollow;

        if (cardInUse && Input.GetMouseButtonUp(0)){
            ConsumeCard();
        }
        else if (cardInUse == false){
            CheckCardSelection();
        }
    }

    private void CheckCardSelection(){
        _activeCard.Card = null;
        _activeCard.CurrentState = ActiveCard.State.None;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f)){
            if (hit.collider.gameObject.CompareTag("Card")){
                int cardIndex = int.Parse(hit.collider.gameObject.name);
                _activeCard.Card = _cardsInHand[cardIndex];
                _activeCard.CurrentState = Input.GetMouseButtonDown(0) ?
                    ActiveCard.State.CursorFollow :
                    ActiveCard.State.CursorHover;
            }
        }
    }

    private void ConsumeCard(){
        _activeCard.CurrentState = ActiveCard.State.None;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            if (hit.transform.TryGetComponent<InteractiveObject>(out var interactiveObject))
            {
                var cardDesc = _activeCard.Card.CardDescriptor;
                var effectObject = Instantiate(cardDesc.EffectPrefab);
                effectObject.transform.position = hit.transform.position;
                Destroy(effectObject, effectObject.GetComponent<ParticleSystem>().main.duration);

                _playerController.Spellcast(hit.transform);

                cardDesc.CardEffectHandler.Handle(hit.transform.gameObject, interactiveObject.Type);
            }
        }
        _cardsInHand.Remove(_activeCard.Card);
        Destroy(_activeCard.Card.SpawnedObject, 0.125f);
        UpdateCardsLayout();
    }
    //------------------------------------------------------------------------



    //------------------------ApplyCardStateOnTransform-----------------------
    private void ApplyCardStateOnTransform(){
        foreach (CardInHand card in _cardsInHand){

            Transform spawnedTransform = card.SpawnedObject.transform;

            bool cardAffected = _activeCard.Card == card && _activeCard.CurrentState != ActiveCard.State.None;
            if (cardAffected){
                if (_activeCard.CurrentState == ActiveCard.State.CursorHover){
                    HighlightCard(card);
                }
                else if (_activeCard.CurrentState == ActiveCard.State.CursorFollow){
                   CardCursorFollowing(spawnedTransform);
                }
            }
            else{
                WaveCard(card);
            }
        }
    }

    private void HighlightCard(CardInHand card){
        Transform spawnedTransform = card.SpawnedObject.transform;
        spawnedTransform.localPosition = Vector3.Slerp(
            spawnedTransform.localPosition, 
            card.BasePosition - spawnedTransform.forward * 0.5f, 
            _lerpTime);
    }

    private void CardCursorFollowing(Transform spawnedTransform){
         Vector3 worldMousePos = _camera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            4.0f
        ));

        RaycastHit targetObject = TargetObject(~LayerMask.GetMask("Cards"));
        if (targetObject.collider != null){
            if (targetObject.transform.TryGetComponent<InteractiveObject>(out var interactiveObject)){
                interactiveObject.TurnOutlineEffect();
            }
        }
        Vector3 newScale = ScaleByDepth(targetObject);
        spawnedTransform.localScale = Vector3.Lerp(spawnedTransform.localScale, newScale, _scaleFactor * Time.deltaTime);

        Vector3 localMousePos = transform.InverseTransformPoint(worldMousePos);

        spawnedTransform.localPosition = new Vector3(
            localMousePos.x + _cursorFollowOffset,
            localMousePos.y,
            spawnedTransform.localPosition.z
        );

        spawnedTransform.localEulerAngles = new Vector3(
            0, 0, Mathf.LerpAngle(spawnedTransform.localEulerAngles.z, 0.0f, _lerpTime));
    }

    private void WaveCard(CardInHand card){
        Vector3 flow = FlowEffect(_amplitude, card.flowOffset);
        Transform spawnedTransform = card.SpawnedObject.transform;
       
        spawnedTransform.localPosition = Vector3.Slerp(
            spawnedTransform.localPosition, card.BasePosition + flow, _lerpTime);

        spawnedTransform.localEulerAngles = new Vector3(
            0, 0, Mathf.LerpAngle(spawnedTransform.localEulerAngles.z, card.BaseRotation, _lerpTime));

        spawnedTransform.localScale = new Vector3(1.0f, 0.8f, 0.4f);
    }

    private Vector3 FlowEffect(float amplitude, Vector3 offset){
       return amplitude * new Vector3(
            Mathf.Sin(Time.time + offset.x) - 1f, 
            Mathf.Sin(Time.time + offset.y) - 1f,
            Mathf.Sin(Time.time + offset.z) - 1f);
    }

    private RaycastHit TargetObject(int layerMask){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance = 100f;
        RaycastHit hit;
        bool successHit = Physics.Raycast(ray, out hit, rayDistance, layerMask);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, successHit? Color.green : Color.red);
        return hit;
    }

    private Vector3 ScaleByDepth(RaycastHit targetObject){
        float depth = targetObject.collider != null ?  ((targetObject.point.z - 1f) -  _camera.transform.position.z) : 1f;
        float scaleValue = Mathf.Clamp(1f / depth, _minScale, _maxScale);
        return new Vector3(scaleValue, scaleValue, scaleValue);
    }
    //------------------------------------------------------------------------
}
