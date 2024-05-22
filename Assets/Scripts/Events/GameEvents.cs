using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents {
  public static event EventHandler OnGameInit;
  public static void GameInit(object sender) => OnGameInit?.Invoke(sender, EventArgs.Empty);

  public static event EventHandler OnGameStarting;
  public static void GameStarting(object sender) => OnGameStarting?.Invoke(sender, EventArgs.Empty);

  public static event EventHandler OnGameStarted;
  public static void GameStarted(object sender) => OnGameStarted?.Invoke(sender, EventArgs.Empty);

  public static event Action<bool, int> OnRoundOver;
  public static void RoundOver(bool hasWon, int playerId) => OnRoundOver?.Invoke(hasWon, playerId);
  
  public static event Action<int> OnGameOver;
  public static void GameOver(int winningPlayerId) => OnGameOver?.Invoke(winningPlayerId);

  public static event Action OnTogglePause;
  public static void TogglePause() => OnTogglePause?.Invoke();
}
