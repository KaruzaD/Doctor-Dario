using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuInput : MonoBehaviour {
  private void Update() {
    if (Input.anyKeyDown) {
      GameSceneManager.Instance.Load(GameSceneManager.Scenes.GameSetup);
      return;
    }
  }
}
