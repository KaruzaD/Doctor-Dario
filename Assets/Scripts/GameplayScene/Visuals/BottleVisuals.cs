using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleVisuals : MonoBehaviour {
  [SerializeField] Playfield playfield;

  [SerializeField] Transform bottleBlockPrefab;
  [SerializeField] Transform bottleCapPrefab;

  [SerializeField] Sprite[] topRightBottomLeftEdges;
  [SerializeField] Sprite[] topleftToprightBottomrightBottomleftCorners;

  private PlayfieldEvents events => playfield.Events;

  private int playfieldWidth;
  private int playfieldHeight;

  private void Start() {
    playfieldWidth = playfield.Width;
    playfieldHeight = playfield.Height;

    DrawBottle();
  }

  private void DrawBottle() {
    DrawCap();
    //top
    DrawSide(topRightBottomLeftEdges[0], playfieldWidth, 0, 1, false);
    //right
    DrawSide(topRightBottomLeftEdges[1], playfieldHeight, playfieldWidth, 0, true);
    //bottom
    DrawSide(topRightBottomLeftEdges[2], playfieldWidth, 0, playfieldHeight * -1, false);
    //left
    DrawSide(topRightBottomLeftEdges[3], playfieldHeight, -1, 0, true);

    //corners
    DrawCorner(topleftToprightBottomrightBottomleftCorners[0], -1, 1);
    DrawCorner(topleftToprightBottomrightBottomleftCorners[1], playfieldWidth, 1);
    DrawCorner(topleftToprightBottomrightBottomleftCorners[2], playfieldWidth, playfieldHeight * -1);
    DrawCorner(topleftToprightBottomrightBottomleftCorners[3], -1, playfieldHeight * -1);
  }

  private void DrawSide(Sprite sprite, int length, int startingX, int startingY, bool isVertical) {
    for (int i = 0; i < length; i++) {
      var tile = Instantiate(bottleBlockPrefab, transform);
      var renderer = tile.GetComponent<SpriteRenderer>();
      renderer.sprite = sprite;

      int xPos = isVertical ? startingX : startingX + i;
      int yPos = isVertical ? (startingY - i) : startingY;

      tile.transform.localPosition = new Vector3(xPos, yPos);
    }
  }

  private void DrawCap() {
    var cap = Instantiate(bottleCapPrefab, transform);
    cap.localPosition = new Vector3(playfield.Center, 0);
  }

  private void DrawCorner(Sprite sprite, int xPos, int yPos) {
    var corner = Instantiate(bottleBlockPrefab, transform);
    var renderer = corner.GetComponent<SpriteRenderer>();
    renderer.sprite = sprite;
    
    corner.transform.localPosition = new Vector3(xPos, yPos);
  }
}
