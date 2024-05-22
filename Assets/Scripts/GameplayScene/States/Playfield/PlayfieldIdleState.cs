using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldIdleState : PlayfieldState {
  
  private GameInputActions.GameplayActions actions => StateMachine.Actions.Gameplay;
  public PlayfieldIdleState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    EnableInput();
    
  }

  public override void Exit() {
    base.Exit();
    DisableInput();
  }

  public override void Update() {
    base.Update();
    if (RunTickTimer()) { return; }
    PollInput();

    if (HandleMove()) { return; }
    HandleFlip();
  }

  public override void HandleTick() {
    base.HandleTick();
    StateMachine.Push(Owner.PillFallingState);
  }

  private void EnableInput() {
    //StateMachine.Actions.Gameplay.Move.started += Move_started;
    //StateMachine.Actions.Gameplay.FlipClockwise.started += FlipClockwise_started;
    //StateMachine.Actions.Gameplay.FlipCounterClockwise.started += FlipCounterClockwise_started;
  }

  

  private void DisableInput() {
    //StateMachine.Actions.Gameplay.Move.started -= Move_started;
    //StateMachine.Actions.Gameplay.FlipClockwise.started -= FlipClockwise_started;
    //StateMachine.Actions.Gameplay.FlipCounterClockwise.started -= FlipCounterClockwise_started;
  }

  private void Move_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    HandleMove();
  }

  private void FlipCounterClockwise_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAndPush(Owner.PillRotateState);
  }

  private void FlipClockwise_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAndPush(Owner.PillRotateState);
  }

  /// <summary>
  /// Checks for pill rotation input and returns true if found; false otherwise.
  /// </summary>
  /// <returns></returns>
  private bool HandleFlip() {
    if (!rotateLeftInput && !rotateRightInput) {
      return false;
    }
    StateMachine.PopAndPush(Owner.PillRotateState);
    return true;
  }

  /// <summary>
  /// Checks for movement input and returns true if detected; false otherwise
  /// </summary>
  /// <returns></returns>
  private bool HandleMove() {
    if (xInput != 0) {
      StateMachine.PopAndPush(Owner.PillMovingState);
      return true;
    } else if (yInput < 0) {
      StateMachine.PopAndPush(Owner.SpeedFallState);
      return true;
    }

    return false;
  }
}
