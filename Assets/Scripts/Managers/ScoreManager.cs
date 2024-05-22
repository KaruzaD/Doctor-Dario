using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
  public static ScoreManager Instance { get; private set; }

  private Dictionary<int, int> wins;
  private Dictionary<int, int> losses;

  /// <summary>
  /// How many rounds won or lost does it take to win or lose the game
  /// </summary>
  private int winThreshhold = 3;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    wins = new Dictionary<int, int>();
    losses = new Dictionary<int, int>();
  }

  private void OnEnable() {
    GameEvents.OnGameInit += GameEvents_OnGameInit;
  }

  private void OnDestroy() {
    GameEvents.OnGameInit -= GameEvents_OnGameInit;
  }

  private void GameEvents_OnGameInit(object sender, System.EventArgs e) {
    wins.Clear();
    losses.Clear();
  }

  public void PlayerRoundOver(int playerIndex, bool hasWon) {
    GameEvents.RoundOver(hasWon, playerIndex);
    
    if (hasWon) {
      IncreaseWins(playerIndex);
    } else {
      IncreaseLosses(playerIndex);
    }
    
  }

  private void IncreaseWins(int playerIndex) {
    if (wins.ContainsKey(playerIndex)) {
      wins[playerIndex]++;
    } else {
      wins.Add(playerIndex, 1);
    }
    CheckGameWin(playerIndex);
  }

  private void IncreaseLosses(int playerIndex) {
    if (losses.ContainsKey(playerIndex)) {
      losses[playerIndex]++;
    }
    else {
      losses.Add(playerIndex, 1);
    }
    CheckGameLoss(playerIndex);
  }

  private void CheckGameWin(int playerIndex) { 
    if (wins[playerIndex] >= winThreshhold) {
      GameEvents.GameOver(playerIndex);
    }
  }

  private void CheckGameLoss(int playerIndex) {
    if (losses[playerIndex] < winThreshhold) {
      return;
    }

    var playersLeft = PlayersStillInGame;
    if (playersLeft.Count <= 0) {
      //single player: lost
      GameEvents.GameOver(-1);
    }
    else if (playersLeft.Count == 1) {
      //multiplayer: another player won
      GameEvents.GameOver(playersLeft[0]);
    }
  }

  public List<int> PlayersStillInGame { get {
      List<int> stillInGame = new List<int>();
      foreach (var playerLosses in losses) {
        if (playerLosses.Value < winThreshhold) {
          stillInGame.Add(playerLosses.Key);
        }
      }
      return stillInGame;
    } }
}
