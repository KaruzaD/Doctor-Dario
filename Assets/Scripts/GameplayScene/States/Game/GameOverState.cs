using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : State<GameManager> {
  public GameOverState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();

    if (Owner.Players.Count == 0) {
      return;
    }

    Owner.Players[0].Actions.UI.Submit.performed += Submit_performed;
  }

  private void Submit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    GameSceneManager.Instance.Load(GameSceneManager.Scenes.GameSetup);
  }

  public override void Exit() {
    base.Exit();

    DisableInput();
  }

  public override void Update() {
    base.Update();
  }

  private void DisableInput() {
    if (Owner.Players.Count == 0) {
      return;
    }

    Owner.Players[0].Actions.UI.Submit.performed -= Submit_performed;
  }
}
