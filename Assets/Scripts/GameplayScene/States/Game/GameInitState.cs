using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitState : State<GameManager> {
  public GameInitState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  private float startDelay = 0.01f;

  public override void Enter() {
    base.Enter();

    Owner.InitPlayfields();
    stateTimer = startDelay;
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();
    
    //Give the players a few frames to initialize
    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    }

    GameEvents.GameInit(Owner);
    StateMachine.PopAndPush(Owner.GameStartingState);
  }
}

