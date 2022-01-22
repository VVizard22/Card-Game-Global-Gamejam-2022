using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Un repositorio para todos los scriptable objects. 
/// Crea tus propios metodos de query aca para mantener la business logic limpia
/// </summary>
public class ResourceSystem : Singleton<ResourceSystem>
{
    
    public List<ScriptableExampleHero> ExampleHeroes { get; private set; }
    private Dictionary<ExampleHeroType, ScriptableExampleHero> _ExampleHeroesDict;
    protected override void Awake(){
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources(){
        ExampleHeroes = Resources.LoadAll<ScriptableExampleHero>("ExampleHeroes").ToList();
        _ExampleHeroesDict = ExampleHeroes.ToDictionary(r => r.HeroType, r => r);
    }

    public ScriptableExampleHero GetExampleHero(ExampleHeroType t) => _ExampleHeroesDict[t];
    public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];
}
