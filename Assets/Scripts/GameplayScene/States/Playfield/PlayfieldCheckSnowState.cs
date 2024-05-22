using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayfieldCheckSnowState : State<Playfield> {
  /// <summary>
  /// The amount of matched colors needed to snow another player
  /// </summary>
  private int matchedColorThreshhold = 2;
  
  public PlayfieldCheckSnowState(Playfield owner, StateMachine<Playfield> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();
    
  }

  public override void Update() {
    if (HandleBeingSnowed()) {
      return;
    }

    HandleSnowingOthers();

    StateMachine.PopAndPush(Owner.PillSpawningState);
  }

  /// <summary>
  /// Pops the next set of colors on the snow queue and returns true if there is any blocks to snow; false otherwise.
  /// </summary>
  /// <returns></returns>
  private bool HandleBeingSnowed() {
    var blocksToFall = new List<Block>();
    
    if (Owner.SnowedBlockColors.TryDequeue(out var blockColors)) {
      switch(Owner.SnowLevel) {
        default:
        case SnowLevel.Off:
          Owner.SnowedBlockColors.Clear();
          return false;
        case SnowLevel.Low:
          int randIndex = UnityEngine.Random.Range(0, blockColors.Count);
          blocksToFall.Add(SnowNewBlock(blockColors[randIndex]));
          break;
        case SnowLevel.Med:
          foreach (var color in blockColors) {
            blocksToFall.Add(SnowNewBlock(color));
          }
          break;
        case SnowLevel.Hi:
          foreach (var color in blockColors) {
            blocksToFall.Add(SnowNewBlock(color));
          }
          randIndex = UnityEngine.Random.Range(0, blockColors.Count);
          blocksToFall.Add(SnowNewBlock(blockColors[randIndex]));
          break;
      }
    } else {
      return false;
    }

    Owner.Events.BlocksPlaced(Owner, blocksToFall);
    Owner.Events.Snowing(Owner, Owner.ID);
    StateMachine.PopAndPush(Owner.BlocksFallingState);
    return true;
  }

  private Block SnowNewBlock(BlockColor color) {
    var gridPosition = Owner.GetRandomUnoccupiedPosition(new System.Random(), highY: 1);
    var block = new Block(color, gridPosition);
    Owner.BlocksFalling[block.Id] = block;
    Owner.PlaceBlockInGrid(block);
    return block;
  }

  private void HandleSnowingOthers() {
    if (Owner.MatchedBlockColors.Count >= matchedColorThreshhold) {
      PlayfieldManager.Instance.SnowRandomPlayer(Owner.ID, Owner.MatchedBlockColors);
    }

    Owner.MatchedBlockColors.Clear();
  }
}
