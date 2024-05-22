using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumbersUI : MonoBehaviour {
  [SerializeField] GameObject[] playerNumberTexts;

  private void Awake() {
    foreach(var text in playerNumberTexts) {
      text.SetActive(false);
    }
  }

  private void Start() {
    PlayerManager.Instance.Events.OnPlayerJoined += Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft += Events_OnPlayerLeft;

    for (int i = 0; i < PlayerManager.Instance.Players.Count; i++) {
      playerNumberTexts[i].SetActive(true);
    }
  }

  private void OnDestroy() {
    PlayerManager.Instance.Events.OnPlayerJoined -= Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft -= Events_OnPlayerLeft;
  }

  private void Events_OnPlayerJoined(UnityEngine.InputSystem.PlayerInput arg1, Player arg2) {
    if (arg1.playerIndex >= playerNumberTexts.Length) {
      return;
    }
    playerNumberTexts[arg1.playerIndex].SetActive(true);
  }

  private void Events_OnPlayerLeft(UnityEngine.InputSystem.PlayerInput arg1, Player arg2) {
    if (arg1.playerIndex >= playerNumberTexts.Length) {
      return;
    }
    playerNumberTexts[arg1.playerIndex].SetActive(false);
  }
}
