using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase se encarga de almacenar individualmente la info de CADA CARTA que 
/// compone un mazo
/// </summary>
public class DecksManager : Singleton<DecksManager>
{
    public Dictionary<string, int> TechDeck { get; private set; } = new Dictionary<string, int>(){
        {"Robosierra", 3},
        {"Robotanque", 3}
    };

    public Dictionary<string, int> MagicDeck { get; private set; } = new Dictionary<string, int>(){
        {"Vidente", 6}
    };

    public void RemoveCard(string name, int amount){
        if(TechDeck.ContainsKey(name)){
            if(TechDeck[name] > amount)
                TechDeck[name] -= amount;
            else
                TechDeck.Remove(name);
            return;
        }
        if(MagicDeck.ContainsKey(name)){
            if(MagicDeck[name] > amount)
                MagicDeck[name] -= amount;
            else
                MagicDeck.Remove(name);
            return;
        }
    }

    public void AddCard(CardBase card){
        if(ResourceSystem.Instance._cardsList.Contains(card)){
            if(card._cardType == CardType.Magic){
                if(MagicDeck.ContainsKey(card._cardName))
                    MagicDeck[card._cardName] += 1;
                else
                    MagicDeck.Add(card._cardName, 1);
                return;
            }
            if(card._cardType == CardType.Tech){
                if(TechDeck.ContainsKey(card._cardName))
                    TechDeck[card._cardName] += 1;
                else
                    TechDeck.Add(card._cardName, 1);
                return;
            }
        }
    }

}
