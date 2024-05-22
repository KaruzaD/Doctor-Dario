using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour {
  private void Awake() {
    GameEvents.OnTogglePause += GameEvents_OnTogglePause;
    gameObject.SetActive(false);
  }

  private void Start() {
    
  }

  private void OnDestroy() {
    GameEvents.OnTogglePause -= GameEvents_OnTogglePause;
  }

  private void GameEvents_OnTogglePause() {
    gameObject.SetActive(!gameObject.activeSelf);
  }
}
