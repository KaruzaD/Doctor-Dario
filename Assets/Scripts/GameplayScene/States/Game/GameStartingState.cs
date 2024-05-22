using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartingState : State<GameManager> {
  private float startDelaySeconds = 2.0f;
  
  public GameStartingState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    Randomishness.NewRandomSeeds();
    foreach (var player in Owner.Players) {
      player.NewRandomGenerators();
    }

    stateTimer = startDelaySeconds;

    GameEvents.GameStarting(Owner);
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    }

    StateMachine.PopAndPush(Owner.GamePlayingState);
  }
}
