using System;
using UnityEngine;


/// <summary>
/// Game manager facil de entender basado en enums, para proyectos mas grandes
/// se puede utilizar el State machine pattern.
/// </summary>
public class ExampleGameManager : Singleton<ExampleGameManager>{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public GameState State { get; private set; }


    public delegate void GameplayPauseHandler(PauseState newPauseState);
    public event GameplayPauseHandler OnGamePauseStateChanged;
    public PauseState pauseState { get; private set; } = PauseState.Play;

    void Start() => ChangeState(GameState.Starting);

    public void ChangePauseState(){
        switch (pauseState){
            case PauseState.Play:
                pauseState = PauseState.Pause;
                break;
            case PauseState.Pause:
                pauseState = PauseState.Play;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pauseState), pauseState, null);
        }
        OnGamePauseStateChanged?.Invoke(pauseState);
    }

    public void ChangeState(GameState newState){
        if(pauseState == PauseState.Pause) return;
        //if(State == newState) return;
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;

        switch (newState){
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningHeroes:
                break;
            case GameState.SpawningEnemies:
                break;
            case GameState.HeroTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"New state: {newState}");
    }

    private void HandleStarting(){
        //Hacer un setup de inicio, sea enviroment, cinematicas, etc.

        //Eventualmente llamar el metoda ChangeState de nuevo con el proximo estado
    }
}

/// <summary>
/// Esto es claramente un ejemplo en cuanto a los distintos estados de un juego
/// pero se puede alterar para hacer casi cualquier tipo de juego o controlar
/// estados de un menu o cinematicas dinamicas, etc.
/// </summary>
[Serializable]
public enum GameState {
    Nothing,
    Starting = 0,
    SpawningHeroes = 1,
    SpawningEnemies = 2,
    HeroTurn = 3,
    EnemyTurn = 4,
    Win = 5,
    Lose = 6
}

[Serializable]
public enum PauseState {
    Play,
    Pause
}