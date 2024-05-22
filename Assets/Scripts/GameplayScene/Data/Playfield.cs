using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;


public class Playfield : MonoBehaviour {
  [SerializeField] public Player Player;

  public int ID { get => Player.ID; }

  public Dictionary<string, Block> BlocksFalling = new Dictionary<string, Block>();
  public Dictionary<string, Block> BlocksLanded = new Dictionary<string, Block>();
  public Queue<List<BlockColor>> SnowedBlockColors = new Queue<List<BlockColor>>();

  #region States
  public PlayfieldStateMachine StateMachine { get; private set; }
  public PlayfieldBlocksFallingState BlocksFallingState { get; private set; }
  public PlayfieldBlocksLandingState BlocksLandingState { get; private set; }
  public PlayfieldPillSpawningState PillSpawningState { get; private set; }
  public PlayfieldIdleState IdleState { get; private set; }
  public PlayfieldPillMovingState PillMovingState { get; private set; }
  public PlayfieldPillRotateState PillRotateState { get; private set; }
  public PlayfieldSpeedFallState SpeedFallState { get; private set; }
  public PlayfieldPillFallingState PillFallingState { get; private set; }
  public PlayfieldCheckSnowState CheckSnowState { get; private set; }


  #endregion

  #region Grid info
  public Block[,] Grid;
  public Pill CurrentPill { get; private set; }
  public Pill NextPill { get; private set; }

  public Vector3 StartPosition;
  public int Width { get => Player.Details.playfieldWidth; }
  public int Height { get => Player.Details.playfieldHeight; }
  public int VirusLevel { get => Player.Details.virusLevel; }
  public SnowLevel SnowLevel { get => Player.Details.snowLevel; }
  public TickRate tickRate;
  
  public float TickRateSeconds { get => tickRate.TickRateSeconds; }
  
  /// <summary>
  /// The number of blocks in a row or column that need to match to break them
  /// </summary>
  private int MatchRequirement { get; set; } = 4;
  public Random VirusRNG { get => Player.VirusRNG; }
  public Random BlockRNG { get => Player.BlockRNG; }
  public int Center { get => Width / 2 - 1; }
  #endregion

  #region Events 
  public PlayfieldEvents Events = new PlayfieldEvents();
  public PlayerEvents PlayerEvents { get => Player.Events; }
  #endregion

  #region Playfield Data
  private int virusesRemaining = 0;
  public int VirusesRemaining { get => virusesRemaining; set {
      virusesRemaining = value;
      Events.VirusCountUpdated(value);
    } }
  public List<BlockColor> MatchedBlockColors = new List<BlockColor>();

  
  private int pillsSpawned = 0;
  public int PillsSpawned { get => pillsSpawned; set {
      pillsSpawned = value;
      tickRate.UpdateTickRate(value);
    } }
  #endregion

  private void Awake() {
    
  }

  private void Start() {
    //Setup Events
    GameEvents.OnGameInit += GameEvents_OnGameInit;
    GameEvents.OnGameStarting += GameEvents_OnGameStarting;
    GameEvents.OnGameStarted += GameEvents_OnGameStarted;

    InitGrid();
  }

  private void OnDestroy() {
    GameEvents.OnGameInit -= GameEvents_OnGameInit;
    GameEvents.OnGameStarting -= GameEvents_OnGameStarting;
    GameEvents.OnGameStarted -= GameEvents_OnGameStarted;

    StateMachine.Destroy();
  }

  public void SetupStates() {
    StateMachine = new PlayfieldStateMachine(this);
    BlocksFallingState = new PlayfieldBlocksFallingState(this, StateMachine, "PlayfieldBlocksFalling");
    BlocksLandingState = new PlayfieldBlocksLandingState(this, StateMachine, "PlayfieldBlocksLanding");
    PillSpawningState = new PlayfieldPillSpawningState(this, StateMachine, "PlayfieldPillSpawning");
    IdleState = new PlayfieldIdleState(this, StateMachine, "PlayfieldIdleState");
    PillMovingState = new PlayfieldPillMovingState(this, StateMachine, "PlayfieldPillMovingState");
    PillRotateState = new PlayfieldPillRotateState(this, StateMachine, "PlayfieldPillRotateState");
    SpeedFallState = new PlayfieldSpeedFallState(this, StateMachine, "PlayfieldSpeedFallState");
    PillFallingState = new PlayfieldPillFallingState(this, StateMachine, "PlayfieldPillFallingState");
    CheckSnowState = new PlayfieldCheckSnowState(this, StateMachine, "PlayfieldCheckSnowState");
  }

  #region Event Handlers
  private void GameEvents_OnGameInit(object sender, EventArgs e) {
    //InitGrid();
  }

  private void GameEvents_OnGameStarting(object sender, EventArgs e) {
    SetupPlayfield();
    CreateNextPill();
  }

  private void GameEvents_OnGameStarted(object sender, EventArgs e) {
    //DropPill();
    StateMachine.PopAllAndPush(PillSpawningState);
  }
  #endregion

  /// <summary>
  /// This is the update method called from the player object so that the playfield is not updated when it is not needed
  /// </summary>
  public void HandleUpdate() {
    StateMachine.CurrentState?.Update();
  }

  public void SetPosition(Vector3 newPosition) {
    transform.localPosition = newPosition;
    //offset the playfield object to center it on its location on the screen
    float offset = Width / 2.0f;
    transform.localPosition -= new Vector3(offset, 0);
  }

  public void InitGrid() {
    Events.InitGrid(this, Width, Height);
  }

  public void SetupPlayfield() {
    Events.ClearGrid(this);
    Grid = new Block[Width, Height];
    tickRate = new TickRate(Player.Details.dropSpeed);

    List<Block> newViruses = new List<Block>();
    for (int i = 0; i < virusesPerDifficulty; i++) {
      var virus = CreateRandomVirus();
      PlaceBlockInGrid(virus);
      newViruses.Add(virus);
    }

    VirusesRemaining = virusesPerDifficulty;
    MatchedBlockColors.Clear();
    BlocksLanded.Clear();
    BlocksFalling.Clear();
    SnowedBlockColors.Clear();
    CurrentPill = null;
    PillsSpawned = 0;

    Events.BlocksPlaced(this, newViruses);
  }

  public void Snowed(List<BlockColor> blockColors) {
    SnowedBlockColors.Enqueue(blockColors);
  }

  public void RoundOver(bool hasWon) {
    ScoreManager.Instance.PlayerRoundOver(ID, hasWon);
  }

  private Virus CreateRandomVirus() {
    var position = GetRandomUnoccupiedPosition(lowY: virusMaxHeight);
    var virus = new Virus(VirusRNG, position);
    return virus;
  }

  public Pill CreateNextPill() {
    NextPill = new Pill(BlockRNG);
    NextPill.SetPosition(new Vector2Int(Center, -1));
    Events.BlocksPlaced(this, NextPill.ActiveBlocks, isNextPill: true);

    return NextPill;
  }

  public bool SpawnPill() {
    if (GetBlockAt(new Vector2Int(Center, 0)) != null) {
      return false;
    }

    CurrentPill = NextPill;
    PlacePillAtTop();

    CreateNextPill();
    return true;
  }

  private void PlacePillAtTop() {
    CurrentPill.SetPosition(new Vector2Int(Center, 0));
    Events.BlocksPlaced(this, CurrentPill.ActiveBlocks, isPill: true);
  }

  public Vector2Int GetRandomUnoccupiedPosition(Random randomGenerator = null, 
    int lowX = 0, int? highX = null, int lowY = 0, int? highY = null) {
    randomGenerator ??= VirusRNG;
    highX ??= Width;
    highY ??= Height;
    
    bool isOccupied = true;
    Vector2Int position = Vector2Int.zero;
    int attempts = 0;
    int maxAttempts = 1000;
    while (isOccupied && attempts < maxAttempts) {
      var randomX = randomGenerator.Next(lowX, highX.Value);
      var randomY = randomGenerator.Next(lowY, highY.Value);
      
      position = new Vector2Int(randomX, randomY);
      isOccupied = GetBlockAt(position) != null;
      attempts++;
    }
    return position;
  }

  /// <summary>
  /// Detaches all the partner blocks, allows them to fall, and removes all blocks from the grid, letting the UI know to update
  /// </summary>
  /// <param name="blocks"></param>
  public void ClearBlocks(List<Block> blocks) {
    blocks.ForEach((block) => {
      if (block.HasPartner) {
        block.Partner.Partner = null;
      }

      if (block is Virus) {
        VirusesRemaining--;
        if (VirusesRemaining < 1) {
          RoundOver(true);
        }
      }

      RemoveBlockFromGrid(block);
    });

    MarkUnsupportedBlocksAsFalling(blocks);

    Events.BlocksRemoved(this, blocks);
  }

  public void MarkUnsupportedBlocksAsFalling(List<Block> clearedBlocks) {
    foreach (var block in clearedBlocks) {
      MarkUnsupportedHelper(GetBlockAt(block.GridPosition + new Vector2Int(-1, 0)));
      MarkUnsupportedHelper(GetBlockAt(block.GridPosition + new Vector2Int(1, 0)));
      MarkUnsupportedHelper(GetBlockAt(block.GridPosition + new Vector2Int(0, -1)));
    }

  }

  private void MarkUnsupportedHelper(Block block) {
    if (block is null) return;
    if (!CanMoveDown(block)) {
      return;
    }

    if (block.IsPartnerHorizontal) {
      if (!CanMoveDown(block.Partner.GridPosition)) {
        return;
      }
      BlocksFalling[block.Id] = block;
    } else {
      BlocksFalling[block.Id] = block;
    }
  }

  public bool IsGrounded(Block block, bool isPartner = false) {
    if (block is null) {
      return false;
    }

    if (block is Virus
      || block.GridPosition.y == Height - 1) {
      return true;
    }

    var downBlock = GetBlockAt(block.GridPosition + new Vector2Int(0, 1));

    if (isPartner || !block.IsPartnerHorizontal) {
      return IsGrounded(downBlock);
    }
    else {
      return IsGrounded(downBlock) || IsGrounded(block.Partner, true);
    }
  }

  public void StickPill() {
    var pillBlocks = CurrentPill.ActiveBlocks;
    pillBlocks.ForEach(block => { 
      PlaceBlockInGrid(block);
      BlocksLanded[block.Id] = block;
    });

    Events.PillLanded(this, CurrentPill);
    CurrentPill = null;
  }

  /// <summary>
  /// Will determine if there is a match in any direction from block.
  /// Will return all the matched blocks (including block) if there is a successful match, else null.
  /// </summary>
  /// <param name="block">The block you want to check against</param>
  /// <returns></returns>
  public void CheckMatch(Block block, HashSet<Block> matchedBlocks) {
    //var matchedBlocks = new HashSet<Block>() { block };

    //check horizontal
    var matchedHorizontal = GetMatchingBlocksInDirection(block, -1, 0);
    matchedHorizontal.AddRange(GetMatchingBlocksInDirection(block, 1, 0));
    if (matchedHorizontal.Count >= MatchRequirement - 1) {
      var oldCount = matchedBlocks.Count;
      matchedBlocks.Add(block);
      matchedBlocks.AddRange(matchedHorizontal);
      if (oldCount != matchedBlocks.Count) {
        MatchedBlockColors.Add(matchedHorizontal.First().Color);
      }
    }

    //check vertical
    var matchedVertical = GetMatchingBlocksInDirection(block, 0, -1);
    matchedVertical.AddRange(GetMatchingBlocksInDirection(block, 0, 1));
    if (matchedVertical.Count >= MatchRequirement - 1) {
      var oldCount = matchedBlocks.Count;
      matchedBlocks.Add(block);
      matchedBlocks.AddRange(matchedVertical);
      if (oldCount != matchedBlocks.Count) {
        MatchedBlockColors.Add(matchedVertical.First().Color);
      }
    }

    //if (matchedBlocks.Count < MatchRequirement) {
    //  return new HashSet<Block>();
    //}
    //
    //return  matchedBlocks;
  }

  private List<Block> GetMatchingBlocksInDirection(Block block, int xDir = 0, int yDir = 0) {
    var matchedBlocks = new List<Block>();

    for (int i = 1; i < MatchRequirement; i++) {
      var position = block.GridPosition + new Vector2Int(i * xDir, i * yDir);
      var checkBlock = GetBlockAt(position);
      if (checkBlock == null || checkBlock.Color != block.Color) {
        break;
      }

      matchedBlocks.Add(checkBlock);
    }

    return matchedBlocks;
  }

  public bool MovePillDown() {
    if (!CanMoveDown(CurrentPill)) { return false; }
    var oldPositions = CurrentPill.BlockPositions;
    CurrentPill.MoveDown();
    Events.PillMoved(this, CurrentPill, CurrentPill.middle.GridPosition);
    return true;
  }

  public bool MovePillLeft() {
    if (!CanMoveLeft(CurrentPill)) { return false; }
    var oldPositions = CurrentPill.BlockPositions;
    CurrentPill.MoveLeft();
    Events.PillMoved(this, CurrentPill, CurrentPill.middle.GridPosition, true);
    return true;
  }

  public bool MovePillRight() {
    if (!CanMoveRight(CurrentPill)) { return false; }
    var oldPositions = CurrentPill.BlockPositions;
    CurrentPill.MoveRight();
    Events.PillMoved(this, CurrentPill, CurrentPill.middle.GridPosition, true);
    return true;
  }

  public void RotatePill(bool clockwise) {
    if (CurrentPill == null) { return; }
    
    if (CurrentPill.orientation == PillDirection.horizontal && !CanMoveUp(CurrentPill)) {
      if (!MovePillDown())
        return;
    }

    var willHitWall = CurrentPill.orientation == PillDirection.vertical 
      && !CanMoveRight(CurrentPill.middle.GridPosition);
    if (willHitWall && !MovePillLeft()) {
      return;
    }
    
    if (clockwise) {
      CurrentPill.RotateRight();
    } else {
      CurrentPill.RotateLeft();
    }

    Events.PillRotated(this, CurrentPill, clockwise);
  }

  public HashSet<Block> GetConnectedBlocks(Block block) {
    var connectedBlocks = new HashSet<Block>() { block };

    GetConnectedBlocksHelper(block, connectedBlocks);

    return connectedBlocks;
  }

  private void GetConnectedBlocksHelper(Block block, HashSet<Block> connectedBlocks, bool isPartner = false) {
    if (block is null || block is Virus) {
      return;
    }
    if (!isPartner && block.IsPartnerHorizontal && IsGrounded(block.Partner, true)) {
      return;
    }
    //if (connectedBlocks.Contains(block)) {
    //  return;
    //}

    connectedBlocks.Add(block);

    var upBlock = GetBlockAt(block.GridPosition + new Vector2Int(0, -1));
    GetConnectedBlocksHelper(upBlock, connectedBlocks);

    if (!isPartner && block.IsPartnerHorizontal && CanMoveDown(block.Partner.GridPosition)) {
      GetConnectedBlocksHelper(block.Partner, connectedBlocks, true);
    }
  }

  public void MoveBlocksDown(List<Block> blocks) {
    var oldPositions = new List<Vector2Int>();

    foreach (var block in blocks) {
      oldPositions.Add(block.GridPosition);
      RemoveBlockFromGrid(block);
      block.MoveDown();
      PlaceBlockInGrid(block);
    }

    Events.BlocksMoved(this, 
      oldPositions, 
      blocks);
  }

  public bool MoveBlockLeft(Block block) {
    if (!CanMoveLeft(block.GridPosition)) { return false; }
    var oldPosition = block.GridPosition;
    RemoveBlockFromGrid(block);
    block.MoveLeft();
    PlaceBlockInGrid(block);
    Events.BlocksMoved(this,
      new List<Vector2Int>(1) { oldPosition },
      new List<Block>(1) { block });
    return true;
  }

  public bool MoveBlockRight(Block block) {
    if (!CanMoveRight(block.GridPosition)) { return false; }
    var oldPosition = block.GridPosition;
    RemoveBlockFromGrid(block);
    block.MoveRight();
    PlaceBlockInGrid(block);
    Events.BlocksMoved(this,
      new List<Vector2Int>(1) { oldPosition },
      new List<Block>(1) { block });
    return true;
  }

  private bool CanMoveUp(Vector2Int position) {
    position += new Vector2Int(0, -1);
    var block = GetBlockAt(position);
    if (position.y < 0 || block != null) {
      return false;
    }

    return true;
  }

  private bool CanMoveUp(Pill pill) {
    var blocks = pill.ActiveBlocks;
    return CanMoveUp(blocks[0].GridPosition) && CanMoveUp(blocks[1].GridPosition);
  }

  private bool CanMoveDown(Vector2Int position) {
    position += new Vector2Int(0, 1);
    var block = GetBlockAt(position);
    if (position.y == Height || block != null) {
      return false; 
    }

    return true;  
  }

  private bool CanMoveDown(Block block) {
    if (block is Virus) {
      return false;
    }
    return CanMoveDown(block.GridPosition);
  }

  private bool CanMoveDown(Pill pill) {
    if (pill == null) { return false; }

    var blocks = pill.ActiveBlocks;
    return CanMoveDown(blocks[0].GridPosition) && CanMoveDown(blocks[1].GridPosition); 
  }

  private bool CanMoveLeft(Vector2Int position) {
    position += new Vector2Int(-1, 0);
    if (position.x < 0 || GetBlockAt(position) != null) {
      return false;
    }

    return true;
  }

  private bool CanMoveLeft(Pill pill) {
    var blocks = pill.ActiveBlocks;
    return CanMoveLeft(blocks[0].GridPosition) && CanMoveLeft(blocks[1].GridPosition);
  }

  private bool CanMoveRight(Vector2Int position) {
    position += new Vector2Int(1, 0);
    if (position.x == Width || GetBlockAt(position) != null) {
      return false;
    }

    return true;
  }

  private bool CanMoveRight(Pill pill) {
    var blocks = pill.ActiveBlocks;
    return CanMoveRight(blocks[0].GridPosition) && CanMoveRight(blocks[1].GridPosition);
  }

  public void PlaceBlockInGrid(Block block) {
    Grid[block.GridPosition.x, block.GridPosition.y] = block;
  }

  private void RemoveBlockFromGrid(Block block) {
    Grid[block.GridPosition.x, block.GridPosition.y] = null;
  }

  private Block GetBlockAt(Vector2Int position) {
    try {
      return Grid[position.x, position.y];
    } catch {
      return null;
    }
  }

  private int virusesPerDifficulty { get {
      return Math.Abs((VirusLevel + 1) * 4);
    }
  }

  private int virusMaxHeight { get {
      if (VirusLevel <= 14) {
        return Height - (int)(Height * 0.625);
      } else if (VirusLevel <= 16) {
        return Height - (int)(Height * 0.625) - 1;
      } else if (VirusLevel <= 18) {
        return Height - (int)(Height * 0.625) - 2;
      } else {
        return Height - (int)(Height * 0.625) - 3;
      }
    }
  }
}
