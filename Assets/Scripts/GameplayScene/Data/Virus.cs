using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Block {
  public Virus(System.Random randomGenerator, Vector2Int gridPosition) : base(randomGenerator, gridPosition) {
  }
  public Virus(BlockColor color, Vector2Int gridPosition) : base(color, gridPosition) {
  }

  public override bool IsStuck { get => true; }
}
