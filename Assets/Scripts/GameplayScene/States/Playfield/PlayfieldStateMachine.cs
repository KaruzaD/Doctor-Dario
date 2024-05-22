using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldStateMachine : StateMachine<Playfield> {

  public GameInputActions Actions;
  public float TickTimer { get; set; }

  public Vector2 PillMoveVector;
  public bool FlipClockwisePressed;
  public bool FlipCounterClockwisePressed;

  
  
  public PlayfieldStateMachine(Playfield owner) : base(owner) {
    //ResetTickTimer();

    Actions = Owner.Player.Actions;
  }

  public override void Destroy() {
    
  }

  public void ResetTickTimer() {
    TickTimer = Owner.TickRateSeconds;
  }




}
