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
    int handCounter = 0;

    void Start()
    {
        _currentHand = new CardBehaviour[6];
        _techDeck = new DeckClass(DecksManager.Instance.TechDeck);
        _magicDeck = new DeckClass(DecksManager.Instance.MagicDeck);
        TurnDraw();
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
        for(int i = 0; i < 3; i++){
            DrawTechCard();
        }
        for(int i = 0; i < 3; i++){
            DrawMagicCard();
        }
    }

    void InstantiateCard(CardBase card){
        GameObject cardCreated;
        switch(card._cardType){
            case CardType.Magic:
                cardCreated = Instantiate(_magicCard, _mDeckTransform.position, Quaternion.identity);
                cardCreated.GetComponent<CardCreation>().SetCardData(card);
                cardCreated.GetComponent<CardBehaviour>().SetCanvas(_playArea.GetComponent<Canvas>());
                cardCreated.transform.SetParent(_playArea.transform);
                CheckEmptySpace(cardCreated);
                _currentHand[handCounter] = cardCreated.GetComponent<CardBehaviour>();
                handCounter++;
                break;
            case CardType.Tech:
                cardCreated = Instantiate(_techCard, _tDeckTransform.position, Quaternion.identity);
                cardCreated.GetComponent<CardCreation>().SetCardData(card);
                cardCreated.GetComponent<CardBehaviour>().SetCanvas(_playArea.GetComponent<Canvas>());
                cardCreated.transform.SetParent(_playArea.transform);
                CheckEmptySpace(cardCreated);
                _currentHand[handCounter] = cardCreated.GetComponent<CardBehaviour>();
                handCounter++;
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
        private static System.Random rng = new System.Random();

        public DeckClass(Dictionary<string, int> deckReference)
        {
            foreach (KeyValuePair<string, int> entry in deckReference)
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
            int head = inGameDeck.Count - 1;
            CardBase aux = null;
            if(head >= 0){
                aux = inGameDeck[head];
                inGameDeck.RemoveAt(head);
            }

            return aux;
        }
    }
}
