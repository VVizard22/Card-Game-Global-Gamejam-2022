using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDeckSystems : MonoBehaviour
{
    [SerializeField] SlotBehaviour[] _slots;
    [SerializeField] Transform _mDeckTransform;
    [SerializeField] Transform _tDeckTransform;
    [SerializeField] GameObject _playArea;
    [SerializeField] GameObject _magicCard;
    [SerializeField] GameObject _techCard;

    private DeckClass _magicDeck;
    private DeckClass _techDeck;

    public CardBehaviour[] _currentHand { get; private set; }


    void OnDestroy()
    {
        ExampleGameManager.OnBeforeStateChanged -= CheckGameState;
        CardBehaviour.OnCardDeath -= DeathActions;
        //SlotBehaviour.OnEmptyingSlot -= DecreaseHandCounter;
    }

    void CheckGameState(GameState state){
        if(state == GameState.Draw)
            TurnDraw();
    }

    void Awake()
    {
        _currentHand = new CardBehaviour[6];
        _techDeck = new DeckClass(DecksManager.Instance.TechDeck);
        _magicDeck = new DeckClass(DecksManager.Instance.MagicDeck);
        ExampleGameManager.OnBeforeStateChanged += CheckGameState;
        CardBehaviour.OnCardDeath += DeathActions;
        //SlotBehaviour.OnEmptyingSlot += DecreaseHandCounter;
    }

    void DeathActions(GameObject card){
        //DiscardCard(card);
        DecreaseHandCounter(card);
    }

    /*
    void DiscardCard(GameObject card){
        if(card.GetComponent<CardBehaviour>()._cardData._cardType == CardType.Magic)
            _magicDeck.SendToDiscardPile(card.GetComponent<CardBehaviour>()._cardData);
        else
            _techDeck.SendToDiscardPile(card.GetComponent<CardBehaviour>()._cardData);
    }*/

    void DecreaseHandCounter(GameObject card){
        bool done = false;
        for(int i = 0; i < _currentHand.Length && !done; i++){
            if(_currentHand[i] == card.GetComponent<CardBehaviour>()){
                _currentHand[i] = null;
                done = true;
            }
        }
    }

    public void DrawMagicCard(){
        CardBase currentMCard = _magicDeck.Draw();
        if(currentMCard != null)
            InstantiateCard(currentMCard);

    }

    public void DrawTechCard(){
        CardBase currentTCard = _techDeck.Draw();
        if(currentTCard != null)
            InstantiateCard(currentTCard);

    }

    void TurnDraw(){
        foreach(SlotBehaviour slot in _slots){
            if(slot.GetSlotType() == CardType.Magic && slot._objectAttatched == null)
                DrawMagicCard();
            else if(slot.GetSlotType() == CardType.Tech && slot._objectAttatched == null)
                DrawTechCard();
        }
    }

    void UpdateCurrentHand(){
        foreach(SlotBehaviour s in _slots){

        }
    }

    void InstantiateCard(CardBase card){
        int pos = -1;
        GameObject cardCreated;
        switch(card._cardType){
            case CardType.Magic:
                cardCreated = Instantiate(_magicCard, _mDeckTransform.position, Quaternion.identity);
                cardCreated.GetComponent<CardCreation>().SetCardData(card);
                cardCreated.GetComponent<CardBehaviour>().SetCanvas(_playArea.GetComponent<Canvas>());
                cardCreated.transform.SetParent(_playArea.transform);
                CheckEmptySpace(cardCreated);

                for(int i = 0; i < _currentHand.Length && pos == -1; i++)
                    if(_currentHand[i] == null)
                        pos = i;
                _currentHand[pos] = cardCreated.GetComponent<CardBehaviour>();
                break;
            case CardType.Tech:
                cardCreated = Instantiate(_techCard, _tDeckTransform.position, Quaternion.identity);
                cardCreated.GetComponent<CardCreation>().SetCardData(card);
                cardCreated.GetComponent<CardBehaviour>().SetCanvas(_playArea.GetComponent<Canvas>());
                cardCreated.transform.SetParent(_playArea.transform);
                CheckEmptySpace(cardCreated);

                for(int i = 0; i < _currentHand.Length && pos == -1; i++)
                    if(_currentHand[i] == null)
                        pos = i;
                _currentHand[pos] = cardCreated.GetComponent<CardBehaviour>();
                break;
        }
    }



    void CheckEmptySpace(GameObject card){
        bool filled = false;
        for(int i = 0; i < _slots.Length && !filled; i++){
            if(_slots[i]._objectAttatched == null){
                _slots[i].FillSlot(card);
                card.GetComponent<RectTransform>().anchoredPosition = _slots[i].anchoredPos;
                filled = true;
            }
        }
    }

    public class DeckClass
    {
        private List<CardBase> inGameDeck = new List<CardBase>();
        private List<CardBase> discardPile = new List<CardBase>();
        private static System.Random rng = new System.Random();
        private Dictionary<string, int> deckInfo;

        public DeckClass(Dictionary<string, int> deckReference) {
            deckInfo = deckReference;

            fillDeck();
        }

        private void fillDeck() {
            foreach (KeyValuePair<string, int> entry in deckInfo)
            {
                for (int i = 1; i <= entry.Value; i++)
                {
                    inGameDeck.Add(ResourceSystem.Instance.GetCard(entry.Key).Duplicate());
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            int n = inGameDeck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardBase value = inGameDeck[k];
                inGameDeck[k] = inGameDeck[n];
                inGameDeck[n] = value;
            }
        }

        public CardBase Draw()
        {
            int head = inGameDeck.Count -1;
            CardBase aux = null;
            if (head >= 0)
            {
                aux = inGameDeck[head];
                inGameDeck.RemoveAt(head);

                discardPile.Add(aux);
            }else {
                fillDeck();
            }

            return aux;
        }
    }
}