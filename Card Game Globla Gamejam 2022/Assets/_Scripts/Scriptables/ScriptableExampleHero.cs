using System;
using UnityEngine;


/// <summary>
/// Create a scriptable Hero
/// </summary>

[CreateAssetMenu(fileName = "New Scriptable Example")]
public class ScriptableExampleHero : ScriptableExampleUnitBase
{
    public ExampleHeroType HeroType;
}

[Serializable]
public enum ExampleHeroType {
    Hero0 = 0,
    Hero1 = 1,
    Hero2 = 2
}
