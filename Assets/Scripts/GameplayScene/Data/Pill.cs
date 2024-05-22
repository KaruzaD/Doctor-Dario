using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Pill {
  public Block top;
  public Block middle;
  public Block right;

  public PillDirection orientation = PillDirection.horizontal;

  public Pill(System.Random randomGenerator) {
    middle = new Block(randomGenerator);
    right = new Block(randomGenerator);

    middle.Partner = right;
    right.Partner = middle;
  }

  public Pill(Block left, Block right) {
    this.middle = left;
    this.right = right;

    middle.Partner = this.right;
    this.right.Partner = middle;
  }

  public void SetPosition(Vector2Int middlePosition) {
    middle.GridPosition = middlePosition;
    if (top != null) {
      top.GridPosition = new Vector2Int(middlePosition.x, middlePosition.y - 1);
    } else {
      right.GridPosition = new Vector2Int(middlePosition.x + 1, middlePosition.y);
    }
  }

  public void MoveDown() {
    middle?.MoveDown();
    top?.MoveDown();
    right?.MoveDown();
  }

  public void MoveRight() {
    middle?.MoveRight();
    top?.MoveRight();
    right?.MoveRight();
  }

  public void MoveLeft() {
    middle?.MoveLeft();
    top?.MoveLeft();
    right?.MoveLeft();
  }

  public void RotateRight(bool hitWall = false) { 
    if (orientation == PillDirection.horizontal) {
      middle.MoveUp();
      top = middle;
      
      right.MoveLeft();
      middle = right;
      right = null;

      orientation = PillDirection.vertical;
    } else {
      top.MoveDownRight();
      right = top;
      top = null;

      if (hitWall) {
        MoveLeft();
      }
      
      orientation = PillDirection.horizontal;
    }
  }

  public void RotateLeft(bool hitWall = false) {
    if (orientation == PillDirection.horizontal) {
      right.MoveUpLeft();
      top = right;
      right = null;

      orientation = PillDirection.vertical;
    }
    else {
      middle.MoveRight();
      right = middle;

      top.MoveDown();
      middle = top;
      top = null;

      if (hitWall) {
        MoveLeft();
      }
      
      orientation = PillDirection.horizontal;
    }
  }

  public List<Block> ActiveBlocks { get {
      var blocks = new List<Block>();
      if (orientation == PillDirection.vertical) {
        blocks.Add(middle);
        blocks.Add(top);
        
      } else {
        blocks.Add(middle);
        blocks.Add(right);
      }
      return blocks;
    } }
  
  public List<Vector2Int> BlockPositions { get {
      var blocks = ActiveBlocks;
      return new List<Vector2Int>() { blocks[0].GridPosition, blocks[1].GridPosition };
    } }
}

public enum PillDirection { vertical, horizontal }
