using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game manager facil de entender basado en enums, para proyectos mas grandes
/// se puede utilizar el State machine pattern.
/// </summary>
public class ExampleGameManager : Singleton<ExampleGameManager>{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    [SerializeField] private CardBase[] EnemyCards;
    [SerializeField] private GameObject EnemySlot;
    [SerializeField] private GameObject EnemyMagicPrefab;
    [SerializeField] private GameObject EnemyTechPrefab;
    [SerializeField] private GameObject CombatSlot;
    [SerializeField] private GameObject FusionScene;

    [SerializeField] private AttackButton attackButton;


    public delegate void GameplayPauseHandler(PauseState newPauseState);
    public event GameplayPauseHandler OnGamePauseStateChanged;
    public PauseState pauseState { get; private set; } = PauseState.Play;

    void Start() {
        SlotBehaviour.StartFusing += StartFusing;
        FusionBehaviour.finishFusion += FinishingFusion;
        CardBehaviour.OnCardDeath += CardDeathCheck;
        ChangeState(GameState.Start);
        CreateEnemyCard();
    }

    void OnDestroy(){
        FusionBehaviour.finishFusion -= FinishingFusion;
        SlotBehaviour.StartFusing -= StartFusing;
        CardBehaviour.OnCardDeath -= CardDeathCheck;      
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
        _secondaryFusing.GetComponent<CardBehaviour>().DestroyCard();
        FusionScene.SetActive(false);
        //ChangeState(GameState.Damage);
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

    public void StartAttackState() => ChangeState(GameState.Damage);

    public void ChangeState(GameState newState){
        //if(pauseState == PauseState.Pause) return;
        //if(State == newState) return;
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;

        switch (newState){
            case GameState.Start:
                HandleStarting();
                break;
            case GameState.Draw:
                HandleDraw();
                break;
            case GameState.TurnStart:
                break;
            case GameState.Fusion:
                HandleFusion();
                break;
            case GameState.Damage:
                HandleDamage();
                break;
            case GameState.CheckForDeaths:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        //Debug.Log($"New state: {newState}");
    }

    public void CheckActivation(){
        attackButton.CheckActivation(CombatSlot.GetComponent<SlotBehaviour>());
    }

    private void HandleStarting(){
        //Hacer un setup de inicio, sea enviroment, cinematicas, etc.
        //Eventualmente llamar el metoda ChangeState de nuevo con el proximo estado
        ChangeState(GameState.Draw);
    }

    private void HandleDraw(){
        ChangeState(GameState.TurnStart);
    }

    private void HandleFusion(){
        FusionScene.SetActive(true);
        //FusionScene.transform.SetAsLastSibling();
    }

    private void HandleDamage(){
        CardCreation enemyCard = null;
        CardCreation allyCard = null;

        enemyCard = EnemySlot.GetComponent<SlotBehaviour>()._objectAttatched.GetComponent<CardCreation>();
        allyCard = CombatSlot.GetComponent<SlotBehaviour>()._objectAttatched.GetComponent<CardCreation>();


        enemyCard._cardData.Damage(allyCard._cardData._baseStats.AttackPower);
        allyCard._cardData.Damage(enemyCard._cardData._baseStats.AttackPower);
        attackButton.GetComponent<Button>().interactable = false;

        allyCard.UpdateCardData();
        enemyCard.UpdateCardData();
        
        ChangeState(GameState.CheckForDeaths);
    }

    private void CreateEnemyCard(){
        System.Random rnd = new System.Random();
        CardBase EnemyToCreate = EnemyCards[rnd.Next(EnemyCards.Length)].Duplicate();
        GameObject EnemyCreated = null;
        if(EnemyToCreate._cardType == CardType.Magic)
            EnemyCreated = Instantiate(EnemyMagicPrefab, EnemySlot.transform.position, Quaternion.identity);
        else if (EnemyToCreate._cardType == CardType.Tech)
            EnemyCreated = Instantiate(EnemyTechPrefab, EnemySlot.transform.position, Quaternion.identity);
        EnemyCreated.GetComponent<CardBehaviour>().SetOnCombatSlot(false);
        EnemyCreated.GetComponent<CardBehaviour>().SetInteractuable(false);
        EnemyCreated.GetComponent<CardCreation>().SetCardData(EnemyToCreate);
        EnemyCreated.transform.SetParent(EnemySlot.transform);
        EnemySlot.GetComponent<SlotBehaviour>().FillSlot(EnemyCreated);
    }

    public void CardDeathCheck(GameObject objectCard){
        CardBehaviour card = objectCard.GetComponent<CardBehaviour>();
        if(card._slotAssignedTo.gameObject == EnemySlot){
            EnemySlot.GetComponent<SlotBehaviour>().EmptySlot();
            CreateEnemyCard();
        }
        if(card._slotAssignedTo.gameObject == CombatSlot){
            CombatSlot.GetComponent<SlotBehaviour>().EmptySlot();
            attackButton.CheckActivation(CombatSlot.GetComponent<SlotBehaviour>());
            ChangeState(GameState.Draw);
        }

        

        
    }
}

/// <summary>
/// Esto es claramente un ejemplo en cuanto a los distintos estados de un juego
/// pero se puede alterar para hacer casi cualquier tipo de juego o controlar
/// estados de un menu o cinematicas dinamicas, etc.
/// </summary>
[Serializable]
public enum GameState {
    Start,
    Draw,
    TurnStart,
    Fusion,
    Damage,
    CheckForDeaths,
    Win,
    Lose
}

[Serializable]
public enum PauseState {
    Play,
    Pause
}