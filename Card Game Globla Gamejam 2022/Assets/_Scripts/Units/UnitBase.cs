using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase es la que comparte la logica de todas las unidades existentes
/// cosas como tomar daño, morir, triggers de animacion, etc.
/// </summary>
public class UnitBase : MonoBehaviour
{
    public Stats Stats { get; private set; }

    public virtual void SetStats(Stats stats) => Stats = stats;

    public virtual void TakeDamage(int dmg) {
        
    }
}
