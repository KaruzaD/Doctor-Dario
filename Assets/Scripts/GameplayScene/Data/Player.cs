using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : HasID {
  [HideInInspector] public GameInputActions Actions;

  public PlayerDetails Details = new PlayerDetails();

  public Playfield Playfield;

  public PlayerEvents Events { get; } = new PlayerEvents();
  public System.Random VirusRNG { get; private set; } = Randomishness.VirusRNG;
  public System.Random BlockRNG { get; private set; } = Randomishness.BlockRNG;

  

  private void Awake() {
    DontDestroyOnLoad(this);

    StateMachine = new PlayerStateMachine(this);
    MenuState = new PlayerMenuState(this, StateMachine, "PlayerMenuState");
    GameplayState = new PlayerGameplayState(this, StateMachine, "PlayerGameplayState");
  }

  private void Start() {
    GoToMenuState();

    GameSceneManager.Instance.Events.OnSceneLoaded += Events_OnSceneLoaded;
    GameEvents.OnGameStarting += GameEvents_OnGameStarting;
    GameEvents.OnGameOver += GameEvents_OnGameOver;
    GameEvents.OnRoundOver += GameEvents_OnRoundOver;
  }

  private void OnDestroy() {
    GameSceneManager.Instance.Events.OnSceneLoaded -= Events_OnSceneLoaded;
    GameEvents.OnGameStarting -= GameEvents_OnGameStarting;
    GameEvents.OnGameOver -= GameEvents_OnGameOver;
    GameEvents.OnRoundOver -= GameEvents_OnRoundOver;

    StateMachine.Destroy();
  }

  private void Events_OnSceneLoaded(GameSceneManager.Scenes scene) {
    switch (scene) {
      default:
      case GameSceneManager.Scenes.GameSetup:
        GoToMenuState();
        break;
      case GameSceneManager.Scenes.Gameplay:
        GoToGameplayState();
        break;
    }
  }

  private void GameEvents_OnRoundOver(bool arg1, int arg2) {
    GoToMenuState();
  }

  private void GameEvents_OnGameOver(int obj) {
    GoToMenuState();
  }

  private void GameEvents_OnGameStarting(object sender, EventArgs e) {
    GoToGameplayState();
  }

  private void OnEnable() {
    
  }

  private void OnDisable() {
    
  }

  public void HandleUpdate() {
    StateMachine.CurrentState?.Update();

  }

  public void GoToGameplayState() {
    StateMachine.PopAndPush(GameplayState);
  }

  public void GoToMenuState() {
    StateMachine.PopAndPush(MenuState);
  }

  public void DetailsChanged() {
    Events.DetailsChanged(Details);
  }

  public void NewRandomGenerators() {
    VirusRNG = Randomishness.VirusRNG;
    BlockRNG = Randomishness.BlockRNG;
  }

  #region States
  public PlayerStateMachine StateMachine { get; private set; }
  public PlayerMenuState MenuState { get; private set; }
  public PlayerGameplayState GameplayState { get; private set; }

  #endregion

}

[Serializable]
public class PlayerDetails {
  public int virusLevel = 0;
  public int playfieldWidth = 8;
  public int playfieldHeight = 16;
  public DropSpeed dropSpeed = DropSpeed.Med;
  public SnowLevel snowLevel = SnowLevel.Med;
}

public enum SnowLevel {
  Off, Low, Med, Hi,
}

public enum DropSpeed {
  Low, Med, Hi,
}
