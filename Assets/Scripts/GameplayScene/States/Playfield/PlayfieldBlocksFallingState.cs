using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class PlayfieldBlocksFallingState : PlayfieldState {
  private float dropTimeSeconds = 0.2f;
  
  public PlayfieldBlocksFallingState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    stateTimer = dropTimeSeconds;
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();
    if (stateTimer > 0) {
      stateTimer -= Time.deltaTime;
      return;
    } else {
      stateTimer = dropTimeSeconds;
      HandleTick();
    }

  }

  public override void HandleTick() {
    //take care of all falling blocks first
    if (Owner.BlocksFalling.Count > 0) {
      HandleFallingBlocks();
      return;
    }

    StateMachine.PopAndPush(Owner.CheckSnowState);
  }

  private void HandleFallingBlocks() {
    var blocksToDrop = new HashSet<Block>();
    var blocksFalling = Owner.BlocksFalling.Values.ToList();

    for (int i = blocksFalling.Count - 1; i >= 0; i--) {
      var block = blocksFalling[i];
      var connectedBlocks = Owner.GetConnectedBlocks(block);
      
      bool isGrounded = Owner.IsGrounded(block);
      if (isGrounded) {
        Owner.BlocksFalling.Remove(block.Id);
        foreach(var connectedBlock in connectedBlocks) {
          Owner.BlocksLanded[connectedBlock.Id] = connectedBlock;
        }

        continue;
      }

      blocksToDrop.AddRange(connectedBlocks);
    }

    if (blocksToDrop.Count > 0) {
      Owner.MoveBlocksDown(blocksToDrop.ToList());
    }

    if (Owner.BlocksLanded.Count > 0) {
      StateMachine.PopAndPush(Owner.BlocksLandingState);
    }
  }
}
