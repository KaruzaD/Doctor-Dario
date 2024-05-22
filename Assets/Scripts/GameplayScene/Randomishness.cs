using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Randomishness {
  private static int virusSeed = 42;
  private static int blockSeed = 69;

  public static Random VirusRNG { get => new MostlyRandom(virusSeed, 4); }
  public static Random BlockRNG { get => new Random(blockSeed); }

  public static void NewRandomSeeds() {
    blockSeed = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() & 0xFFFFFFFF);
    virusSeed = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() & 0xFFFFFFFF);
  }

}

public class MostlyRandom : Random {
  private int lastValue;
  private int sequentialCount;
  private int maxSequentialCount;

  public MostlyRandom(int seed, int maxSequentialCount = 3) : base(seed) {
    this.maxSequentialCount = maxSequentialCount;
  }

  public override int Next(int minValue, int maxValue) {
    if (minValue >= maxValue)
      throw new ArgumentException("minValue must be less than maxValue.");

    int nextValue;
    do {
      nextValue = base.Next(minValue, maxValue);
    } while (nextValue == lastValue && sequentialCount >= maxSequentialCount);

    if (nextValue == lastValue)
      sequentialCount++;
    else
      sequentialCount = 0;

    lastValue = nextValue;
    return nextValue;
  }
}

