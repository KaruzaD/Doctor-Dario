using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePausedState : State<GameManager> {
  public GamePausedState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();


  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    foreach (var player in PlayerManager.Instance.Players.Values) {
      if (player.Actions.Gameplay.Exit.triggered) {
        GameEvents.GameOver(-1);
        GameSceneManager.Instance.Load(GameSceneManager.Scenes.GameSetup);
        return;
      }
    }
  }

  private IEnumerator Exited() {
    GameEvents.GameOver(-1);
    yield return new WaitForSeconds(0.05f);
    GameSceneManager.Instance.Load(GameSceneManager.Scenes.GameSetup);
  }
}
