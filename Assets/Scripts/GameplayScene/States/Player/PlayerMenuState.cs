using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuState : State<Player> {
  public PlayerMenuState(Player owner, StateMachine<Player> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();

    Owner.Actions.UI.Enable();
  }

  public override void Exit() {
    base.Exit();

    Owner.Actions.UI.Disable();
  }

  public override void Update() {
    base.Update();
  }
}
