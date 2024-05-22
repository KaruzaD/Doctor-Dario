using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldPillFallingState : PlayfieldState {
  public PlayfieldPillFallingState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();

    
  }

  public override void Exit() {
    base.Exit();
  }

  public override void HandleTick() {
    
  }

  public override void Update() {

    HandleFallingPill();
  }

  private void HandleFallingPill() {
    if (!Owner.MovePillDown()) {
      Owner.StickPill();
      Owner.StateMachine.PopAllAndPush(Owner.BlocksLandingState);
      return;
    }

    StateMachine.Pop();
  }


}
