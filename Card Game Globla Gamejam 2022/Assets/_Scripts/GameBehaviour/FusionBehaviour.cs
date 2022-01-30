using UnityEngine.UI;
using UnityEngine;
using System;

public class FusionBehaviour : MonoBehaviour
{
    public static event Action<GameObject, GameObject> finishFusion;

    [SerializeField] Button _confirmFussionButton;
    [SerializeField] GameObject _fusionCanvas;
    [SerializeField] InGameDeckSystems _hand;
    [SerializeField] GameObject mainFusion;
    [SerializeField] FusionSlot[] HandSlots;
    [SerializeField] GameObject FusionPrefabMagic;
    [SerializeField] GameObject FusionPrefabTech;

    public GameObject _originalCardToFuse { get; private set; }
    public GameObject _secondaryCardToFuse { get; private set; }

    private Vector3 _cardScale;
    private GameObject _cardToFuse;

    void Awake(){
        _cardScale = new Vector3(0.3f, 0.3f, 1);
        CardToFuse.SelectCard += ActivateSelection;
    }

    void OnDestroy()
    {
        CardToFuse.SelectCard -= ActivateSelection;
    }

    public void SetFusionCard(GameObject card){
        if(card.GetComponent<CardBehaviour>() != null){
            SetMainFusion(card);
            FusionPossibility();
        }
    }

    private void ActivateSelection(GameObject originalCard){
        _secondaryCardToFuse = originalCard;
        
        if(_secondaryCardToFuse != null){
            _confirmFussionButton.interactable = true;
        }
        else
            _confirmFussionButton.interactable = false;
    }

    public void FusingSuccesfull(){
        if(_originalCardToFuse != null && _secondaryCardToFuse != null){
            finishFusion?.Invoke(_originalCardToFuse, _secondaryCardToFuse);
            Destroy(_secondaryCardToFuse);
            CancelFusing();
        }
    }

    public void CancelFusing(){
        _cardToFuse = null;
        mainFusion.GetComponent<FusionSlot>().EmptySlot();
        foreach(FusionSlot s in HandSlots){
            s.EmptySlot();
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("FusionCard")){
            Destroy(g);
        }
        _fusionCanvas.SetActive(false);
        ExampleGameManager.Instance.ChangeState(GameState.TurnStart);
    }


    void FusionPossibility(){
        CardType currentType = _cardToFuse.GetComponent<CardCreation>()._cardData._cardType;
        
        for(int i = 0; i < _hand._currentHand.Length; i++){
            if(_hand._currentHand[i] != null  && _hand._currentHand[i]._cardData._cardType != currentType){
                InstantiateCard(_hand._currentHand[i].gameObject);
            }
        }
    }

    void InstantiateCard(GameObject cardGameObject){
        CardBase card = cardGameObject.GetComponent<CardCreation>()._cardData;
        FusionSlot dedicatedSlot = GetFreeSlot();
        GameObject newCard = null;
        if(card._cardType == CardType.Magic)
            newCard = Instantiate(FusionPrefabMagic, dedicatedSlot.GetComponent<RectTransform>().position, Quaternion.identity);
        else
            newCard = Instantiate(FusionPrefabTech, dedicatedSlot.GetComponent<RectTransform>().position, Quaternion.identity);

        newCard.GetComponent<CardToFuse>().SetOriginalReference(cardGameObject);
        newCard.transform.SetParent(this.transform);
        newCard.GetComponent<RectTransform>().anchoredPosition = dedicatedSlot.GetComponent<RectTransform>().anchoredPosition;
        newCard.transform.localScale = _cardScale;
        
        newCard.GetComponent<CardCreation>().SetCardData(card);        
        dedicatedSlot.FillSlot(newCard);
        newCard.GetComponent<CardCreation>().UpdateCardFusionData();
    }


    void SetMainFusion(GameObject card){
            CardBehaviour current = card.GetComponent<CardBehaviour>();
            GameObject CardToFuse = null;
            
            if(current._cardData._cardType == CardType.Magic)
                CardToFuse = Instantiate(FusionPrefabMagic, mainFusion.transform.position, Quaternion.identity);
            else
                CardToFuse = Instantiate(FusionPrefabTech, mainFusion.transform.position, Quaternion.identity);
            
            CardToFuse.GetComponent<CardToFuse>().SetOriginalReference(card);
            CardToFuse.transform.SetParent(this.transform);
            _cardScale = card.transform.localScale;
            CardToFuse.transform.localScale = _cardScale;
            CardToFuse.GetComponent<RectTransform>().anchoredPosition = mainFusion.GetComponent<RectTransform>().anchoredPosition;
            CardToFuse.GetComponent<CardCreation>().SetCardData(card.GetComponent<CardCreation>()._cardData);
            CardToFuse.GetComponent<CardToFuse>().SetCardData(card.GetComponent<CardCreation>()._cardData);
            CardToFuse.GetComponent<CardToFuse>().SetMainFusion();
            CardToFuse.transform.SetAsLastSibling();

            mainFusion.GetComponent<FusionSlot>().FillSlot(CardToFuse);

            _originalCardToFuse = card;
            _cardToFuse = CardToFuse;
    }
    FusionSlot GetFreeSlot(){
        FusionSlot slot = null;

        for (int i = 0; i < HandSlots.Length; i++){
            FusionSlot current = HandSlots[i];

            if(current._objectAttatched == null)
                slot = current;
        }

        return slot;
    }
}
