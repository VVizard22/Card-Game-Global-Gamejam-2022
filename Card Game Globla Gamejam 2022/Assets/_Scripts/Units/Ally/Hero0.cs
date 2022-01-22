using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero0 : HeroUnitBase
{
    [SerializeField] private AudioClip _someSound;

    void Start(){
        //Ejemplo de como usar un sistema estatico
        AudioSystem.Instance.PlaySound(_someSound);
    }

    public override void ExecuteMove()
    {
        //Hace la animacion especifica, dmg, movimiento, etc. del ataque especial de Hero0.
        //Se necesitara aceptar las cosas especificas de el movimiento como un argumento de esta funcion.
        base.ExecuteMove(); //Se llama el metodo base para pasar en limpio.
    }
}
