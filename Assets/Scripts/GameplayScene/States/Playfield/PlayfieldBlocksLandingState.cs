using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayfieldBlocksLandingState : PlayfieldState {
  public PlayfieldBlocksLandingState(Playfield owner, PlayfieldStateMachine stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();
    Owner.BlocksLanded.Clear();
  }

  public override void Update() {
    HandleLandedBlocks();

    StateMachine.PopAndPush(Owner.BlocksFallingState);
  }

  public override void HandleTick() {
    
  }

  private void HandleLandedBlocks() {
    var matchedBlocks = new HashSet<Block>();
    
    foreach (var blockKVP in Owner.BlocksLanded) {
      Owner.CheckMatch(blockKVP.Value, matchedBlocks);
    }
    
    if (matchedBlocks.Count == 0) {
      return;
    }

    //Update the grid and UI to remove these matched blocks, allowing affected blocks to fall
    Owner.ClearBlocks(matchedBlocks.ToList());
  }

}
