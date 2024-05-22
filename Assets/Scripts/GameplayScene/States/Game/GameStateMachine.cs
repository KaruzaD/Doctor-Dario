using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine<GameManager> {

  public GameStateMachine(GameManager owner) : base(owner) {
  }
  public GameStateMachine(GameManager owner, State<GameManager> startState) : base(owner, startState) {
  }
}
