using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Block {
  public string Id = System.Guid.NewGuid().ToString();
  public BlockColor Color;
  public Vector2Int GridPosition;
  public Transform SpawnedObject;
  /// <summary>
  /// This is the other half of the pill if this is a pill block
  /// </summary>
  public Block Partner;

  public bool HasPartner => Partner != null;
  public bool IsPartnerAbove => HasPartner && Partner.GridPosition.y < GridPosition.y;
  public bool IsPartnerBelow => HasPartner && Partner.GridPosition.y > GridPosition.y;
  public bool IsPartnerRight => HasPartner && Partner.GridPosition.x > GridPosition.x;
  public bool IsPartnerLeft => HasPartner && Partner.GridPosition.x < GridPosition.x;
  public bool IsPartnerHorizontal => IsPartnerLeft || IsPartnerRight;
  
  public Transform Prefab { get; private set; }
  public virtual bool IsStuck { get; set; } = false;

  public Block(BlockColor color, Vector2Int? gridPosition = null) { 
    this.Color = color;
    
    if (gridPosition == null) { gridPosition = Vector2Int.zero; }
    this.GridPosition = gridPosition.Value;
    
  }

  public Block(Random randomGenerator, Vector2Int? gridPosition = null) {
    this.Color = GetRandomColor(randomGenerator);

    if (gridPosition == null) { gridPosition = Vector2Int.zero; }
    this.GridPosition = gridPosition.Value;

  }

  public void MoveRight() {
    GridPosition += new Vector2Int(1, 0);
  }

  public void MoveLeft() {
    GridPosition += new Vector2Int(-1, 0);
  }
  public void MoveUp() {
    GridPosition += new Vector2Int(0, -1);
  }

  public void MoveDown() {
    GridPosition += new Vector2Int(0, 1);
  }

  public void MoveUpLeft() {
    GridPosition += new Vector2Int(-1, -1);
  }

  public void MoveDownRight() {
    GridPosition += new Vector2Int(1, 1);
  }

  public BlockColor GetRandomColor(Random generator) {
    var colors = Enum.GetNames(typeof(BlockColor));
    var index = generator.Next(0, colors.Length);
    var randomColorName = colors[index];
    return (BlockColor)Enum.Parse(typeof(BlockColor), randomColorName);
  }

}

public enum BlockColor {
  red, yellow, blue,
}
