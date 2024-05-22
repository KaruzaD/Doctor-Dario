using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldState : State<Playfield> {
  new public PlayfieldStateMachine StateMachine { get; private set; }

  protected float xInput { get; private set; }
  protected float yInput { get; private set; }
  protected bool rotateRightInput { get; private set; }
  protected bool rotateLeftInput { get; private set; }

  public PlayfieldState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
    StateMachine = stateMachine;
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();
  }

  public virtual void HandleTick() {
  }

  public override void Update() {
    base.Update();
    
  }

  public void PollInput() {
    var moveVector = StateMachine.Actions.Gameplay.Move.ReadValue<Vector2>();
    xInput = moveVector.x;
    yInput = moveVector.y;
    rotateLeftInput = StateMachine.Actions.Gameplay.FlipCounterClockwise.IsPressed();
    rotateRightInput = StateMachine.Actions.Gameplay.FlipClockwise.IsPressed();
  }

  /// <summary>
  /// returns true if has ticked; false otherwise
  /// </summary>
  /// <returns></returns>
  public bool RunTickTimer() {
    if (StateMachine.TickTimer <= 0) {
      StateMachine.ResetTickTimer();

      HandleTick();
      return true;
    }
    else {
      StateMachine.TickTimer -= Time.deltaTime;
    }
    return false;
  }
}
