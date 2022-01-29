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

    [SerializeField] private GameObject CombatSlot;
    [SerializeField] private GameObject FusionScene;


    public delegate void GameplayPauseHandler(PauseState newPauseState);
    public event GameplayPauseHandler OnGamePauseStateChanged;
    public PauseState pauseState { get; private set; } = PauseState.Play;

    void Start() {
        SlotBehaviour.StartFusing += StartFusing;
        FusionBehaviour.finishFusion += FinishingFusion;
        ChangeState(GameState.Starting);
    }

    void OnDestroy()
    {
        FusionBehaviour.finishFusion -= FinishingFusion;
        SlotBehaviour.StartFusing -= StartFusing;        
    }

    void FinishingFusion(GameObject _originalFusing, GameObject _secondaryFusing){
        _originalFusing.GetComponent<CardBehaviour>()._slotAssignedTo.GetComponent<SlotBehaviour>().EmptySlot();
        CombatSlot.GetComponent<SlotBehaviour>().FillSlot(_originalFusing);
        CardCreation originalData = _originalFusing.GetComponent<CardCreation>();
        CardCreation secondaryData = _secondaryFusing.GetComponent<CardCreation>();

        originalData._cardData.SetStats(secondaryData._cardData._fusionStats.Health, secondaryData._cardData._fusionStats.AttackPower);
        originalData.UpdateCardData();
        
        _secondaryFusing.GetComponent<CardBehaviour>()._slotAssignedTo.GetComponent<SlotBehaviour>().EmptySlot();

        _originalFusing.GetComponent<CardBehaviour>().ReturnPosition();
        Destroy(_secondaryFusing);
        FusionScene.SetActive(false);
    }

    void StartFusing(GameObject card){
        FusionScene.GetComponent<FusionBehaviour>().SetFusionCard(card);
        ChangeState(GameState.Fusion);
    }

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
            case GameState.Draw:
                break;
            case GameState.CardSelect:
                break;
            case GameState.Fusion:
                HandleFusion();
                break;
            case GameState.Damage:
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

    private void HandleFusion(){
        FusionScene.SetActive(true);
        //FusionScene.transform.SetAsLastSibling();
    }
}

/// <summary>
/// Esto es claramente un ejemplo en cuanto a los distintos estados de un juego
/// pero se puede alterar para hacer casi cualquier tipo de juego o controlar
/// estados de un menu o cinematicas dinamicas, etc.
/// </summary>
[Serializable]
public enum GameState {
    Starting,
    Draw,
    CardSelect,
    Fusion,
    Damage,
    Win,
    Lose
}

[Serializable]
public enum PauseState {
    Play,
    Pause
}