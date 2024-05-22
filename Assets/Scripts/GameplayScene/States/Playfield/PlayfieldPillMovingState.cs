using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldPillMovingState : PlayfieldState {
  private float inputCooldown = 0.15f;
  private bool doFlipInterrupt = false;
  
  public PlayfieldPillMovingState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    stateTimer = 0;
    doFlipInterrupt = false;
    EnableInput();
  }

  public override void Exit() {
    base.Exit();
    DisableInput();
  }

  public override void HandleTick() {
    base.HandleTick();
    StateMachine.Push(Owner.PillFallingState);
  }

  public override void Update() {
    if (doFlipInterrupt) {
      StateMachine.PopAndPush(Owner.PillRotateState);
      return;
    }
    
    base.Update();
    if (RunTickTimer()) { return; }
    PollInput();

    if (xInput == 0) {
      StateMachine.PopAndPush(Owner.IdleState);
      return;
    }

    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    }

    if (xInput > 0) {
      Owner.MovePillRight();
      stateTimer = inputCooldown;
    }
    else if (xInput < 0) {
      Owner.MovePillLeft();
      stateTimer = inputCooldown;
    }
  }

  private void EnableInput() {
    //StateMachine.Actions.Gameplay.Move.canceled += Move_canceled;
    StateMachine.Actions.Gameplay.FlipClockwise.started += FlipClockwise_started;
    StateMachine.Actions.Gameplay.FlipCounterClockwise.started += FlipCounterClockwise_started;
  }

  private void DisableInput() {
    //StateMachine.Actions.Gameplay.Move.canceled -= Move_canceled;
    StateMachine.Actions.Gameplay.FlipClockwise.started += FlipClockwise_started;
    StateMachine.Actions.Gameplay.FlipCounterClockwise.started += FlipCounterClockwise_started;
  }

  private void FlipCounterClockwise_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //StateMachine.PopAndPush(Owner.PillRotateState);
    doFlipInterrupt = true;
  }

  private void FlipClockwise_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //StateMachine.PopAndPush(Owner.PillRotateState);
    doFlipInterrupt = true;
  }

  private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAndPush(Owner.IdleState);
  }

  
}
