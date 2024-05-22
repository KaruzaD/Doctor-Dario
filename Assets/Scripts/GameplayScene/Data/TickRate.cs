using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TickRate {
  public float TickRateSeconds { get; private set; }

  private DropSpeed dropSpeed;

  private int maxIncreases = 49;
  private int increases = -1;
  private int frameRatePerSecond = 60;

  /// <summary>
  /// Every n pills speeds up the tick rate according to the internal table in TickRate
  /// </summary>
  private int pillsPerSpeedup = 10;

  public TickRate(DropSpeed dropSpeed) {
    this.dropSpeed = dropSpeed;
    UpdateTickRate(0);
  }

  private int[] framesPerRow = new int[] {
    70, 68, 66, 64, 62, 60, 58, 56, 54, 52, 50, 48, 46, 44, 42, 
    40, 38, 36, 34, 32, 30, 28, 26, 24, 22, 
    20, 19, 18, 17, 16, 15, 
    14, 13, 12, 11, 10, 10, 9, 9, 8, 8, 7, 7, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1
  };

  public Dictionary<DropSpeed, int> dropSpeedOffset = new Dictionary<DropSpeed, int>() {
    { DropSpeed.Low, 15 },
    { DropSpeed.Med, 25 },
    { DropSpeed.Hi, 31 },
  };

  public void UpdateTickRate(int pillsSpawned) {
    var oldValue = increases;
    increases = pillsSpawned / pillsPerSpeedup;
    if (increases == oldValue) return;

    TickRateSeconds = CalculateTickRate();
    UnityEngine.Debug.Log($"Tick Rate updated to {TickRateSeconds}");
  }

  private float CalculateTickRate() {
    var speedOffset = dropSpeedOffset[dropSpeed];
    var frames = framesPerRow[speedOffset + increases];

    var tickRate = frames / (float)frameRatePerSecond;
    return tickRate;
  }
}

