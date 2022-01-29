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

    [SerializeField] private Stats _fusionFormStats;
    public Stats _fusionStats => _fusionFormStats;

    public Sprite _cardSprite;

    public void SetStats(int hp, int ap){
        _stats.ModifyStats(hp, ap);
        _baseStats.ModifyStats(_stats.Health, _stats.AttackPower);
    }

    public CardBase Duplicate(){
        CardBase toReturn = CreateInstance<CardBase>();
        
        toReturn._cardType = _cardType;
        toReturn._cardName = _cardName;
        toReturn._cardDescription = _cardDescription;
        toReturn._fusionFormStats = _fusionFormStats;
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

    public void ModifyStats(int hp, int ap){
        Health += hp;
        AttackPower += ap;
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
