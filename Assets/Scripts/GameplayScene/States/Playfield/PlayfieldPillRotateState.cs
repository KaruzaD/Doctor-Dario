using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayfieldPillRotateState : PlayfieldState {
  private float inputCooldown = 0.5f;
  private bool doMoveInterrupt = false;

  public PlayfieldPillRotateState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }


  public override void Enter() {
    base.Enter();
    stateTimer = 0;
    doMoveInterrupt = false;
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
    if (doMoveInterrupt) {
      StateMachine.PopAndPush(Owner.PillMovingState);
    }

    base.Update();
    PollInput();

    if (!rotateLeftInput && !rotateRightInput) {
      StateMachine.PopAndPush(Owner.IdleState);
      return;
    }

    if (RunTickTimer()) { return; }

    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    }

    if (rotateLeftInput) {
      Owner.RotatePill(false);
      stateTimer = inputCooldown;
    }
    else if (rotateRightInput) {
      Owner.RotatePill(true);
      stateTimer = inputCooldown;
    }

  }

  private void EnableInput() {
    StateMachine.Actions.Gameplay.Move.started += Move_started;
    //StateMachine.Actions.Gameplay.FlipClockwise.canceled += FlipClockwise_canceled;
    //StateMachine.Actions.Gameplay.FlipCounterClockwise.canceled += FlipCounterClockwise_canceled;
  }

  private void DisableInput() {
    StateMachine.Actions.Gameplay.Move.started -= Move_started;
    //StateMachine.Actions.Gameplay.FlipClockwise.canceled -= FlipClockwise_canceled;
    //StateMachine.Actions.Gameplay.FlipCounterClockwise.canceled -= FlipCounterClockwise_canceled;
  }

  private void FlipCounterClockwise_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAndPush(Owner.IdleState);
  }

  private void FlipClockwise_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    StateMachine.PopAndPush(Owner.IdleState);
  }

  private void Move_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    doMoveInterrupt = true;
  }
}

