using System;
using UnityEngine;

/// <summary>
/// Mantiene toda la informacion acerca de una unidad en un scriptableobject,
/// esta es la base de scriptableObject para toda unidad
/// </summary>
public abstract class ScriptableExampleUnitBase : ScriptableObject
{
    public Faction faction;
    
    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats;


    //Used in game
    public HeroUnitBase Prefab;

    //Used in Menus
    public string Description;
    public Sprite MenuSprite;
}

/// <summary>
/// Mantener las stats como un struct en el scriptable object lo mantiene flexible y facil de editar.
/// Podemos pasar este struct al prefab spawneado y alterarlo dependiendo en las condiciones
/// </summary>

[Serializable]
public struct Stats{
    public int Health;
    public int AttackPower;
}

[Serializable]
public enum Faction {
    Heroes = 0,
    Enemies = 1
}