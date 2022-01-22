using UnityEngine;

/// <summary>
/// Un ejemplo de un manager especifico de una escena agarrando recursos del sistema de recursos
/// Managers especificos de escena son cosas como grid managers, unit managers, inviroment managers, etc.
/// </summary>
public class ExampleUnitManager : Singleton<ExampleUnitManager>
{
    public void SpawnHeroes(){
        SpawnUnit(ExampleHeroType.Hero0, new Vector3(1, 0, 0));
    }

    void SpawnUnit(ExampleHeroType t, Vector3 pos){
        var Hero0Scriptable = ResourceSystem.Instance.GetExampleHero(t);

        var spawned = Instantiate(Hero0Scriptable.Prefab, pos, Quaternion.identity, transform);

        //Aplicar posibles modificaciones aca, como pociones, boosts, sinergias, etc.
        var stats = Hero0Scriptable.BaseStats;
        stats.Health += 20;

        spawned.SetStats(stats);
    }
}
