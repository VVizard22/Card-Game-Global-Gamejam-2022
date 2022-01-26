using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Un repositorio para todos los scriptable objects. 
/// Crea tus propios metodos de query aca para mantener la business logic limpia
/// </summary>
public class ResourceSystem : Singleton<ResourceSystem>
{
    
    public List<CardBase> _cardsList { get; private set; }
    public List<CardBase> _techList { get; private set; }
    public List<CardBase> _magicList { get; private set; }
    private Dictionary<string, CardBase> _magicCardsDict;
    private Dictionary<string, CardBase> _techCardsDict;
    protected override void Awake(){
        base.Awake();
        _techList = new List<CardBase>();
        _magicList = new List<CardBase>();
        _magicCardsDict = new Dictionary<string, CardBase>();
        _techCardsDict = new Dictionary<string, CardBase>();
        AssembleResources();
    }

    private void AssembleResources(){
        _cardsList = Resources.LoadAll<CardBase>("Cards").ToList();
        foreach(CardBase _card in _cardsList){
            switch (_card._cardType){
                case CardType.Magic:
                    _magicCardsDict.Add(_card._cardName, _card);
                    _magicList.Add(_card);
                    break;
                case CardType.Tech:
                    _techCardsDict.Add(_card._cardName, _card);
                    _techList.Add(_card);
                    break;
            }
        }
        DisplayDictionary();
    }

    public CardBase GetCard(string cardName){
        CardBase cardReturn = null;
        if(_techCardsDict.ContainsKey(cardName))
            cardReturn = _techCardsDict[cardName];
        if(cardReturn == null && _magicCardsDict.ContainsKey(cardName))
            cardReturn = _magicCardsDict[cardName];
        return cardReturn;
    }

    public CardBase GetRandomTechCard() => _techList[Random.Range(0, _techList.Count)];
    public CardBase GetRandomMagicCard() => _magicList[Random.Range(0, _techList.Count)];


    //Metodo para testeo de diccionarios
    private void DisplayDictionary(){
        Debug.Log("Tech dict");
        foreach(CardBase card in _techCardsDict.Values){
            Debug.Log(card._cardName);
        }
        
        Debug.Log("Magic Dict");
        foreach(CardBase card in _magicCardsDict.Values){
            Debug.Log(card._cardName);
        }
    }
}

