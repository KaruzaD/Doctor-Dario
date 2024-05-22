using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoundOverState : State<GameManager> {
  public GameRoundOverState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    if (Owner.Players.Count == 0) {
      return;
    }
    
    foreach (var player in Owner.Players) {
      player.Actions.UI.Submit.performed += Submit_performed;
    }
  }

  private void Submit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAllAndPush(Owner.GameStartingState);
  }

  public override void Exit() {
    base.Exit();

    if (Owner.Players.Count == 0) {
      return;
    }

    foreach (var player in Owner.Players) {
      player.Actions.UI.Submit.performed -= Submit_performed;
    }
  }

  public override void Update() {
    base.Update();
  }
}
