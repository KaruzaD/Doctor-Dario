using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldEvents {
  public PlayfieldEvents() { }

  public event EventHandler<OnInitGridEventArgs> OnInitGrid;
  public class OnInitGridEventArgs : EventArgs {
    public int width;
    public int height;

    public OnInitGridEventArgs(int width, int height) {
      this.width = width;
      this.height = height;
    }
  }
  public void InitGrid(object sender, int width, int height) => OnInitGrid?.Invoke(sender,
    new OnInitGridEventArgs(width, height));

  public event EventHandler OnClearGrid;
  public void ClearGrid(object sender) => OnClearGrid?.Invoke(sender, EventArgs.Empty);

  public event EventHandler<OnBlocksPlacedEventArgs> OnBlocksPlaced;
  public class OnBlocksPlacedEventArgs : EventArgs {
    public List<Block> blocks;
    public bool isPill;
    public bool isNextPill;

    public OnBlocksPlacedEventArgs(List<Block> blocks, bool isPill = false, bool isNextPill = false) {
      this.blocks = blocks;
      this.isPill = isPill;
      this.isNextPill = isNextPill;
    }
  }
  public void BlocksPlaced(object sender, List<Block> blocks, bool isPill = false, bool isNextPill = false) => OnBlocksPlaced?.Invoke(sender,
    new OnBlocksPlacedEventArgs(blocks, isPill, isNextPill));

  public event EventHandler<OnBlocksRemovedEventArgs> OnBlocksRemoved;
  public class OnBlocksRemovedEventArgs : EventArgs {
    public List<Block> blocks;
    public bool isCurrentPill;
    public bool isNextPill;

    public OnBlocksRemovedEventArgs(List<Block> blocks, bool isPill = false, bool isNextPill = false) {
      this.blocks = blocks;
      this.isCurrentPill = isPill;
      this.isNextPill = isNextPill;
    }
  }
  public void BlocksRemoved(object sender, List<Block> blocks, bool isPill = false, bool isNextPill = false) =>
    OnBlocksRemoved?.Invoke(sender, new OnBlocksRemovedEventArgs(blocks, isPill, isNextPill));


  public event EventHandler<OnBlocksMovedEventArgs> OnBlocksMoved;
  public class OnBlocksMovedEventArgs : EventArgs {
    public List<Vector2Int> oldPositions;
    public List<Block> movedBlocks;

    public OnBlocksMovedEventArgs(List<Vector2Int> oldPositions, List<Block> movedBlocks) {
      this.oldPositions = oldPositions;
      this.movedBlocks = movedBlocks;
    }
  }
  public void BlocksMoved(object sender, List<Vector2Int> oldPositions, List<Block> movedBlocks) => OnBlocksMoved?.Invoke(sender,
    new OnBlocksMovedEventArgs(oldPositions, movedBlocks));

  public event Action<Pill> OnNextPillGenerated;
  public void NextPillGenerated(Pill pill) => OnNextPillGenerated?.Invoke(pill);

  public event Action<Pill> OnCurrentPillDropped;
  public void CurrentPillDropped(Pill pill) => OnCurrentPillDropped?.Invoke(pill);

  public event Action<object, Pill, bool> OnPillRotated;
  public void PillRotated(object sender, Pill pill, bool clockwise) => OnPillRotated?.Invoke(sender, pill, clockwise);

  public event Action<object, Pill> OnPillLanded;
  public void PillLanded(object sender, Pill pill) => OnPillLanded?.Invoke(sender, pill);

  public event Action<object, Pill, Vector2Int, bool> OnPillMoved;
  public void PillMoved(object sender, Pill pill, Vector2Int destination, bool isHorizontal = false) => OnPillMoved?.Invoke(sender, pill, destination, isHorizontal);

  public event Action<int> OnVirusCountUpdated;
  public void VirusCountUpdated(int virusesRemaining) => OnVirusCountUpdated?.Invoke(virusesRemaining);

  public event Action<object, int> OnSnowed;
  public void Snowing(object sender, int playerIndex) => OnSnowed?.Invoke(sender, playerIndex);
}
