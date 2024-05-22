using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldSpeedFallState : PlayfieldState {
  private float speedTickRateSeconds = 0.06f;
  private bool doFlipInterrupt = false;

  public PlayfieldSpeedFallState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
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
    StateMachine.ResetTickTimer();
  }

  public override void HandleTick() {
    base.HandleTick();
    stateTimer = speedTickRateSeconds;
    StateMachine.Push(Owner.PillFallingState);
  }

  public override void Update() {
    if (doFlipInterrupt) {
      StateMachine.PopAndPush(Owner.PillRotateState);
      return;
    }
    base.Update();
    PollInput();

    if (yInput == 0) {
      StateMachine.PopAndPush(Owner.IdleState);
      return;
    }

    //if (xInput != 0) {
    //  StateMachine.PopAndPush(Owner.PillMovingState);
    //  return;
    //}
    //
    //if (rotateLeftInput || rotateRightInput) {
    //  StateMachine.PopAndPush(Owner.PillRotateState);
    //  return;
    //}

    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
    } else {
      HandleTick();
      return;
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
