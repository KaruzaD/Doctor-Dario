using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldVisuals : MonoBehaviour {
  [Header("Containers")]
  [SerializeField] private Transform nextPillSpawnArea;
  [SerializeField] private Transform blocksContainer;
  [SerializeField] private Transform backgroundTiles;

  [Header("Prefab references")]
  [SerializeField] private Transform backgroundTilePrefab;
  [SerializeField] private Transform blockPrefab;
  [SerializeField] private Transform virusPrefab;
  [SerializeField] private Transform pillPrefab;

  [Header("Playfield Data")]
  [SerializeField] private Playfield playfield;
  [SerializeField] private LayerMask blocksLayer;

  private PlayfieldEvents events { get => playfield.Events; }
  private PlayerEvents playerEvents { get => playfield.Player.Events; }

  private PillVisuals spawnedPill;
  private PillVisuals spawnedNextPill;
  private PillVisuals pillShadow;

  private int playfieldWidth;
  private int playfieldHeight;

  private bool isPillShadowVisible = false;

  private void Start() {
    SetupEvents();

    int nextPillSpawnAreaXOffset = playfield.Center;
    nextPillSpawnArea.localPosition += new Vector3(nextPillSpawnAreaXOffset, 0);

    InitPlayfield(playfield.Width, playfield.Height);
  }

  public void SetupEvents() {
    events.OnClearGrid += Events_OnClearGrid;
    events.OnInitGrid += Events_OnInitGrid;
    
    events.OnBlocksPlaced += Events_OnBlocksPlaced;
    events.OnBlocksRemoved += Events_OnBlocksRemoved;
    events.OnBlocksMoved += Events_OnBlocksMoved;

    events.OnPillRotated += Events_OnPillRotated;
    events.OnPillMoved += Events_OnPillMoved;
    events.OnPillLanded += Events_OnPillLanded;

    playerEvents.OnTogglePillDropShadow += PlayerEvents_OnTogglePillDropShadow;
  }

  private void OnDestroy() {
    events.OnInitGrid -= Events_OnInitGrid;
    events.OnClearGrid -= Events_OnClearGrid;
    
    events.OnBlocksPlaced -= Events_OnBlocksPlaced;
    events.OnBlocksRemoved -= Events_OnBlocksRemoved;
    events.OnBlocksMoved -= Events_OnBlocksMoved;
    
    events.OnPillRotated -= Events_OnPillRotated;
    events.OnPillMoved -= Events_OnPillMoved;
    events.OnPillLanded -= Events_OnPillLanded;

    playerEvents.OnTogglePillDropShadow += PlayerEvents_OnTogglePillDropShadow;
  }

  #region Event Handlers
  private void Events_OnClearGrid(object sender, System.EventArgs e) {
    ClearPlayfield();
  }

  private void Events_OnInitGrid(object sender, PlayfieldEvents.OnInitGridEventArgs e) {
    
  }

  private void Events_OnBlocksMoved(object sender, PlayfieldEvents.OnBlocksMovedEventArgs e) {
    MoveBlocks(e.oldPositions, e.movedBlocks);
  }

  private void Events_OnBlocksRemoved(object sender, PlayfieldEvents.OnBlocksRemovedEventArgs e) {
    RemoveBlocks(e.blocks);
  }

  private void Events_OnBlocksPlaced(object sender, PlayfieldEvents.OnBlocksPlacedEventArgs e) {
    if (e.isNextPill) {
      ClearNextPill(); 
      DrawNextPill(new Pill(e.blocks[0], e.blocks[1]));
      return;
    } else if (e.isPill) {
      SpawnPill(new Pill(e.blocks[0], e.blocks[1]));
      return;
    }
    SpawnBlocks(e.blocks);
  }

  private void Events_OnPillRotated(object sender, Pill pill, bool clockwise) {
    RotatePill(pill, clockwise);
  }

  private void Events_OnPillMoved(object sender, Pill pill, Vector2Int destination, bool isHorizontal) {
    MovePill(destination);
  }

  private void Events_OnPillLanded(object sender, Pill pill) {
    PillLanded(pill);
  }

  private void PlayerEvents_OnTogglePillDropShadow() {
    isPillShadowVisible = !isPillShadowVisible;
    if (pillShadow == null) {
      return;
    }
    pillShadow.gameObject.SetActive(isPillShadowVisible);
  }
  #endregion


  #region Setup and Teardown
  private void InitPlayfield(int width, int height) {
    playfieldWidth = width;
    playfieldHeight = height;
    
    DrawNextPillBackground();
    DrawMainBackgroundTiles(width, height);
  }

  private void DrawNextPillBackground() {
    var tile1 = Instantiate(backgroundTilePrefab, nextPillSpawnArea);
    var tile2 = Instantiate(backgroundTilePrefab, nextPillSpawnArea);
    tile2.localPosition += new Vector3(1, 0);
  }

  private void DrawMainBackgroundTiles(int width, int height) {
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        var tile = Instantiate(backgroundTilePrefab, backgroundTiles);
        tile.localPosition += new Vector3(x, y * -1); //flipped to match world coordinates with grid coordinates
      }
    }
  }

  private void ClearPlayfield() {
    foreach (Transform block in blocksContainer) {
      Destroy(block.gameObject);
    }

    ClearSpawnedPill();
    ClearNextPill();
  }

  private void ClearSpawnedPill() {
    if (spawnedPill != null) {
      spawnedPill.Remove();
      spawnedPill = null;
    }

    if (pillShadow != null) { 
      pillShadow.Remove();
      pillShadow = null;
    }
  }

  private void ClearNextPill() {
    if (spawnedNextPill != null) {
      spawnedNextPill.Remove();
      spawnedNextPill = null;
    }
  }

  #endregion

  #region Pill Manipulation
  private void DrawNextPill(Pill pill) {
    var pillVisualsTransform = Instantiate(pillPrefab, nextPillSpawnArea);
    spawnedNextPill = pillVisualsTransform.GetComponentInChildren<PillVisuals>();

    spawnedNextPill.Setup(pill);
  }

  private void SpawnPill(Pill pill) {
    var pillVisualsTransform = Instantiate(pillPrefab, transform);
    spawnedPill = pillVisualsTransform.GetComponentInChildren<PillVisuals>();
    //pillVisualsTransform.localPosition += new Vector3(pill.middle.GridPosition.x, 0);
    spawnedPill.PillTransform.localPosition += new Vector3(pill.middle.GridPosition.x, 0);

    spawnedPill.Setup(pill);
    spawnedPill.DropIn(nextPillSpawnArea.position);

    SpawnPillShadow(pill);
  }

  private void SpawnPillShadow(Pill pill) {
    var pillShadowTransform = Instantiate(pillPrefab, transform);
    pillShadow = pillShadowTransform.GetComponentInChildren<PillVisuals>();
    pillShadow.SetupPillShadow(pill);

    pillShadow.gameObject.SetActive(isPillShadowVisible);

    SetPillShadowPosition(spawnedPill.PillTransform.localPosition.x);
  }

  private void SetPillShadowPosition(float xPosition) {
    float startX = spawnedPill.Orientation == PillDirection.vertical
      ? spawnedPill.transform.position.x
      : spawnedPill.transform.position.x + 0.5f;
    var hit = Physics2D.BoxCast(
      new Vector2(startX, spawnedPill.transform.position.y),
      new Vector2(0.5f, 0.5f),
      spawnedPill.transform.eulerAngles.z,
      Vector2.down,
      playfieldHeight + 1,
      blocksLayer);
    
    float yPosition = hit.collider == null ? (playfieldHeight-1) * -1 : hit.collider.transform.localPosition.y+1;

    pillShadow.transform.localPosition = new Vector3(xPosition, yPosition);
  }

  private void PillLanded(Pill pill) {
    SpawnBlocks(pill.ActiveBlocks);

    ClearSpawnedPill();
  }

  private void MovePill(Vector2Int destination) {
    spawnedPill.Move(new Vector3(destination.x, destination.y * -1), 
      onComplete: () => SetPillShadowPosition(destination.x));
    
  }

  private void RotatePill(Pill pill, bool clockwise) {
    spawnedPill.Rotate(pill, clockwise);
    pillShadow.Rotate(pill, clockwise);
    
  }
  #endregion

  #region Block Manipulations
  private void SpawnBlocks(List<Block> blocks) {
    foreach (var block in blocks) {
      Transform tile;
      if (block is Virus) {
        tile = Instantiate(virusPrefab, blocksContainer);
      } else {
        tile = Instantiate(blockPrefab, blocksContainer);
      }
      //y is flipped to match grid indexes to world coordinates
      tile.localPosition += new Vector3(block.GridPosition.x, block.GridPosition.y * -1);
      block.SpawnedObject = tile;

      var blockVisuals = tile.GetComponentInChildren<BlockVisuals>();
      blockVisuals.Setup(block);
    }
  }

  private void RemoveBlocks(List<Block> blocks) {
    foreach (var block in blocks) {
      
      var blockVisuals = block.SpawnedObject.GetComponentInChildren<BlockVisuals>();
      blockVisuals.Remove();
    }
  }

  private void MoveBlocks(List<Vector2Int> oldPositions, List<Block> movedBlocks) {
    for (int i = 0; i < oldPositions.Count; i++) {
      var block = movedBlocks[i];
      var delta = block.GridPosition - oldPositions[i];

      var blockVisuals = block.SpawnedObject?.GetComponentInChildren<BlockVisuals>();
      //y is multiplied by -1 to match grid indexes to world coordinates
      blockVisuals?.Move(new Vector3(block.GridPosition.x, block.GridPosition.y * -1));
    }
  }
  #endregion
}


/// <summary>
///     A class that allows to visualize the Physics2D.BoxCast() method.
/// </summary>
/// <remarks>
///     Use Draw() to visualize an already cast box,
///     and BoxCastAndDraw() to cast a box AND visualize it at the same time.
/// </remarks>


public static class BoxCastDrawer {
  /// <summary>
  ///     Visualizes BoxCast with help of debug lines.
  /// </summary>
  /// <param name="hitInfo"> The cast result. </param>
  /// <param name="origin"> The point in 2D space where the box originates. </param>
  /// <param name="size"> The size of the box. </param>
  /// <param name="angle"> The angle of the box (in degrees). </param>
  /// <param name="direction"> A vector representing the direction of the box. </param>
  /// <param name="distance"> The maximum distance over which to cast the box. </param>
  public static void Draw(
      RaycastHit2D hitInfo,
      Vector2 origin,
      Vector2 size,
      float angle,
      Vector2 direction,
      float distance = Mathf.Infinity) {
    // Set up points to draw the cast.
    Vector2[] originalBox = CreateOriginalBox(origin, size, angle);

    Vector2 distanceVector = GetDistanceVector(distance, direction);
    Vector2[] shiftedBox = CreateShiftedBox(originalBox, distanceVector);

    // Draw the cast.
    Color castColor = hitInfo ? Color.red : Color.green;
    DrawBox(originalBox, castColor);
    DrawBox(shiftedBox, castColor);
    ConnectBoxes(originalBox, shiftedBox, Color.gray);

    if (hitInfo) {
      Debug.DrawLine(hitInfo.point, hitInfo.point + (hitInfo.normal.normalized * 0.2f), Color.yellow);
    }
  }

  /// <summary>
  ///     Casts a box against colliders in the Scene, returning the first collider to contact with it, and visualizes it.
  /// </summary>
  /// <param name="origin"> The point in 2D space where the box originates. </param>
  /// <param name="size"> The size of the box. </param>
  /// <param name="angle"> The angle of the box (in degrees). </param>
  /// <param name="direction"> A vector representing the direction of the box. </param>
  /// <param name="distance"> The maximum distance over which to cast the box. </param>
  /// <param name="layerMask"> Filter to detect Colliders only on certain layers. </param>
  /// <param name="minDepth"> Only include objects with a Z coordinate (depth) greater than or equal to this value. </param>
  /// <param name="maxDepth"> Only include objects with a Z coordinate (depth) less than or equal to this value. </param>
  /// <returns>
  ///     The cast result.
  /// </returns>
  public static RaycastHit2D BoxCastAndDraw(
      Vector2 origin,
      Vector2 size,
      float angle,
      Vector2 direction,
      float distance = Mathf.Infinity,
      int layerMask = Physics2D.AllLayers,
      float minDepth = -Mathf.Infinity,
      float maxDepth = Mathf.Infinity) {
    var hitInfo = Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
    Draw(hitInfo, origin, size, angle, direction, distance);
    return hitInfo;
  }

  private static Vector2[] CreateOriginalBox(Vector2 origin, Vector2 size, float angle) {
    float w = size.x * 0.5f;
    float h = size.y * 0.5f;
    Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

    var box = new Vector2[4]
    {
            new Vector2(-w, h),
            new Vector2(w, h),
            new Vector2(w, -h),
            new Vector2(-w, -h),
    };

    for (int i = 0; i < 4; i++) {
      box[i] = (Vector2)(q * box[i]) + origin;
    }

    return box;
  }

  private static Vector2[] CreateShiftedBox(Vector2[] box, Vector2 distance) {
    var shiftedBox = new Vector2[4];
    for (int i = 0; i < 4; i++) {
      shiftedBox[i] = box[i] + distance;
    }

    return shiftedBox;
  }

  private static void DrawBox(Vector2[] box, Color color) {
    Debug.DrawLine(box[0], box[1], color);
    Debug.DrawLine(box[1], box[2], color);
    Debug.DrawLine(box[2], box[3], color);
    Debug.DrawLine(box[3], box[0], color);
  }

  private static void ConnectBoxes(Vector2[] firstBox, Vector2[] secondBox, Color color) {
    Debug.DrawLine(firstBox[0], secondBox[0], color);
    Debug.DrawLine(firstBox[1], secondBox[1], color);
    Debug.DrawLine(firstBox[2], secondBox[2], color);
    Debug.DrawLine(firstBox[3], secondBox[3], color);
  }

  private static Vector2 GetDistanceVector(float distance, Vector2 direction) {
    if (distance == Mathf.Infinity) {
      // Draw some large distance e.g. 5 scene widths long.
      float sceneWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
      distance = sceneWidth * 5f;
    }

    return direction.normalized * distance;
  }
}
