using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayState : State<Player> {
  public PlayerGameplayState(Player owner, StateMachine<Player> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();

    Owner.Actions.Gameplay.Enable();
    Owner.Actions.Gameplay.ShowHidePillShadow.performed += ShowHidePillShadow_performed;
    Owner.Actions.Gameplay.Pause.performed += Pause_performed;
  }

  private void ShowHidePillShadow_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    Owner.Events.TogglePillDropShadow();
  }

  private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    GameEvents.TogglePause();
  }

  public override void Exit() {
    base.Exit();

    Owner.Actions.Gameplay.ShowHidePillShadow.performed -= ShowHidePillShadow_performed;
    Owner.Actions.Gameplay.Pause.performed -= Pause_performed;

    Owner.Actions.Gameplay.Disable();
  }

  public override void Update() {
    base.Update();


    Owner.Playfield?.HandleUpdate();
  }
}
