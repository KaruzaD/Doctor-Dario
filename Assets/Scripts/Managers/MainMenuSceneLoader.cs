using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneLoader : MonoBehaviour {
  [SerializeField] Transform essentialManagersPrefab;
  [SerializeField] Transform backgroundGridPrefab;

  private void Awake() {
    if (EssentialManagers.Instance == null) { Instantiate(essentialManagersPrefab); }
    if (BackgroundGridVisuals.Instance == null) { Instantiate(backgroundGridPrefab); }
  }
}
