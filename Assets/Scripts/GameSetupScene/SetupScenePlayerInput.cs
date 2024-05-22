using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetupScenePlayerInput : MonoBehaviour {
  private void Start() {

  }

  private void Update() {
    foreach (var player in PlayerManager.Instance.Players.Values) {
      if (player.ID == 0) {
        if (player.Actions.UI.Submit.triggered) {
          GameSceneManager.Instance.Load(GameSceneManager.Scenes.Gameplay);
          return;
        }
      }

      if (player.Actions.UI.Cancel.triggered) {
        PlayerManager.Instance.Remove(player.ID);
        return;
      }
    }
  }

}
