using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusLevelDisplayUI : MonoBehaviour {
  [SerializeField] VirusLevelDisplayBoxUI[] displayBoxes;

  private void Awake() {
    foreach (var box in displayBoxes) {
      box.gameObject.SetActive(false);
    }
  }

  private void Start() {
    PlayerManager.Instance.Events.OnPlayerJoined += Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft += Events_OnPlayerLeft;

    for (int i = 0; i < PlayerManager.Instance.Players.Count; i++) {
      EnableBox(i);
    }
  }

  private void OnDestroy() {
    PlayerManager.Instance.Events.OnPlayerJoined -= Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft -= Events_OnPlayerLeft;
  }

  private void Events_OnPlayerJoined(UnityEngine.InputSystem.PlayerInput arg1, Player arg2) {
    if (arg1.playerIndex >= displayBoxes.Length) {
      return;
    }
    EnableBox(arg1.playerIndex);
  }

  private void EnableBox(int index) {
    var box = displayBoxes[index];
    box.PlayerIndex = index;
    box.gameObject.SetActive(true);
  }

  private void Events_OnPlayerLeft(UnityEngine.InputSystem.PlayerInput arg1, Player arg2) {
    if (arg1.playerIndex >= displayBoxes.Length) {
      return;
    }
    var box = displayBoxes[arg1.playerIndex];
    box.gameObject.SetActive(false);
  }
}
