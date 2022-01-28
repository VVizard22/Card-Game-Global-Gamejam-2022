using System;
using UnityEngine;

[CreateAssetMenu (fileName = "Scriptable Card", menuName = "Cards/New Scriptable Card")]
public class CardBase : ScriptableObject
{
    public CardType _cardType;
    public CardNature _cardNature;
    public string _cardName;

    [TextArea]
    public string _cardDescription;

    [SerializeField] private Stats _stats;
    public Stats _baseStats => _stats;

    public Sprite _cardSprite;

    public CardBase Duplicate(){
        CardBase toReturn = CreateInstance<CardBase>();
        
        toReturn._cardType = _cardType;
        toReturn._cardName = _cardName;
        toReturn._cardDescription = _cardDescription;
        toReturn._stats = _stats;
        toReturn._cardSprite = _cardSprite;

        return toReturn;
    }
}

[Serializable]
public struct Stats{
    public int Health;
    public int AttackPower;

    //Constructor
    public Stats(int hp, int ap){
        Health = hp;
        AttackPower = ap;
    }
}

[Serializable]
public enum CardType{
    Magic,
    Tech
}

[Serializable]
public enum CardNature{
    Minion,
    Spell
}
