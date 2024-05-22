using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldPillSpawningState : PlayfieldState {
  private float spawnDelaySeconds = 0.1f;

  /// <summary>
  /// A check to make sure update is not called any extra amount while waiting for game to change states
  /// </summary>
  private bool roundOverDetected = false;

  public PlayfieldPillSpawningState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    stateTimer = spawnDelaySeconds;
    roundOverDetected = false;

    if (Owner.CurrentPill == null && !Owner.SpawnPill()) {
      Owner.RoundOver(false);
      roundOverDetected = true;
      return;
    }

    Owner.PillsSpawned++;
    StateMachine.ResetTickTimer();
  }

  public override void Exit() {
    base.Exit();
    StateMachine.ResetTickTimer();
  }

  public override void HandleTick() {
  }

  public override void Update() {
    if (roundOverDetected) {
      return;
    }

    //a little delay before dropping the next pill
    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    }

    StateMachine.PopAndPush(Owner.IdleState);
  }
}
