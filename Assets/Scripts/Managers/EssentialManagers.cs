using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialManagers : MonoBehaviour {
  public static EssentialManagers Instance { get; private set; }
  
  private void Awake() {
    DontDestroyOnLoad(this);

    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }
}
