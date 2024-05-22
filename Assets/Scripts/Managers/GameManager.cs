using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public List<Player> Players { get {
      return PlayerManager.Instance.Players.Values.ToList();  
    } 
  }

  public static GameManager Instance { get; private set; }

  void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    DOTween.Init(recycleAllByDefault: true);
    DOTween.defaultAutoPlay = AutoPlay.AutoPlayTweeners;
    
    StateMachine = new GameStateMachine(this);

    GameInitState = new GameInitState(this, StateMachine, "GameInit");
    GameStartingState = new GameStartingState(this, StateMachine, "GameStarting");
    GamePlayingState = new GamePlayingState(this, StateMachine, "GamePlaying");
    GamePausedState = new GamePausedState(this, StateMachine, "GamePaused");
    GameRoundOverState = new GameRoundOverState(this, StateMachine, "GameRoundOver");
    GameOverState = new GameOverState(this, StateMachine, "GameOver");
  }

  private void Start() {
    GameEvents.OnRoundOver += GameEvents_OnRoundOver;
    GameEvents.OnGameOver += GameEvents_OnGameOver;
    GameEvents.OnTogglePause += GameEvents_OnTogglePause;

    StateMachine.PopAndPush(GameInitState);
  }

  private void Update() {
    StateMachine.CurrentState?.Update();
  }

  private void OnDestroy() {
    StateMachine.Destroy();

    GameEvents.OnRoundOver -= GameEvents_OnRoundOver;
    GameEvents.OnGameOver -= GameEvents_OnGameOver;
    GameEvents.OnTogglePause -= GameEvents_OnTogglePause;
  }

  private void GameEvents_OnRoundOver(bool arg1, int arg2) {
    StateMachine.PopAndPush(GameRoundOverState);
  }

  private void GameEvents_OnGameOver(int obj) {
    GameOver();
  }

  private void GameEvents_OnTogglePause() {
    if (StateMachine.CurrentState == GamePausedState) {
      StateMachine.Pop();
    } else {
      StateMachine.Push(GamePausedState);
    }
  }

  private void GameOver() {
    StateMachine.PopAndPush(GameOverState);
  }

  public void InitPlayfields() {
    foreach (var player in Players) {
      PlayfieldManager.Instance.Add(player);
    }
  }

  #region States
  public GameStateMachine StateMachine { get; private set; }
  public GameInitState GameInitState { get; private set; } 
  public GameStartingState GameStartingState { get; private set; } 
  public GamePlayingState GamePlayingState { get; private set; }
  public GamePausedState GamePausedState { get; private set; }
  public GameRoundOverState GameRoundOverState { get; private set; } 
  public GameOverState GameOverState { get; private set; }

  #endregion
}


