using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Esta clase es la que comparte la logica de todas las unidades existentes
/// cosas como tomar daño, morir, triggers de animacion, etc.
/// </summary>
public class CardUnitBase : MonoBehaviour
{
    public static event Action<GameObject> CardUnitDie;


    public Stats _stats { get; private set; } = new Stats(0, 0);

    public virtual void SetStats(Stats stats) => _stats = stats;

    public virtual void TakeDamage(int dmg) {
        Stats afterDamage = _stats;
        afterDamage.Health -= dmg;
        SetStats(afterDamage);
        CheckDeath();
    }

    public virtual void CheckDeath(){
        if(_stats.Health <= 0) {
            CardUnitDie?.Invoke(this.gameObject);
        }
    }
}
