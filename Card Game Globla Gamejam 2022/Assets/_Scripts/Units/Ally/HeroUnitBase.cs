using UnityEngine;


/// <summary>
/// Logica general de control de los personajes que controla el jugador
/// </summary>
public class HeroUnitBase : UnitBase
{
    private bool _canMove;

    private void Awake () => ExampleGameManager.OnBeforeStateChanged += OnStateChanged;

    private void OnDestroy() => ExampleGameManager.OnBeforeStateChanged -= OnStateChanged;

    private void OnStateChanged(GameState newState) {
        if (newState == GameState.HeroTurn) _canMove = true;
    }

    private void OnMOuseDown(){
        //Solo permitir interaccion cuando es el turno del heroe
        if (ExampleGameManager.Instance.State != GameState.HeroTurn) return;

        //No moverse si ya se movio
        if (!_canMove) return;

        //Mostrar opciones de movimiento/Ataque

        //Eventualmente desseleccionar o ExecuteMove(). Se puede dividir ExecuteMove en multiples funciones
        //como Move() / Attack() / Dance()

        Debug.Log("Unit Clicked");
    }

    public virtual void ExecuteMove(){
        //Sobreescribir esto para hacer alguna logica especifica al heroe,
        //Despues llamar a este metodo base para pasar en limpio el turno

        _canMove = false;
    }
}
