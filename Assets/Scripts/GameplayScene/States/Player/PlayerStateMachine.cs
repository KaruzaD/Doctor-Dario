using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<Player> {
  public PlayerStateMachine(Player owner) : base(owner) {
  }

  public PlayerStateMachine(Player owner, State<Player> startState) : base(owner, startState) {
  }
}
