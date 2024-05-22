using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistent<T> : Singleton<T> where T : class {
  protected override void Awake() {
    base.Awake();
    DontDestroyOnLoad(gameObject);
  }
}
