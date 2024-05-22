using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {
  public static GameSceneManager Instance;

  public SceneEvents Events = new SceneEvents();

  public enum Scenes {
    MainMenu, GameSetup, Gameplay,
  }

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

  public void Load(Scenes scene) {
    StartCoroutine(LoadAsync(scene));
  }

  public IEnumerator LoadAsync(Scenes scene) {
    var asyncLoad = SceneManager.LoadSceneAsync((int)scene);
    
    while (!asyncLoad.isDone) {
      yield return null;
    }
    
    Events.SceneLoaded(scene);
  }

  public Scenes ActiveScene => (Scenes)Enum.GetValues(typeof(Scenes))
    .GetValue(SceneManager.GetActiveScene().buildIndex);
}

public class SceneEvents {

  public event Action<GameSceneManager.Scenes> OnSceneLoaded;
  public void SceneLoaded(GameSceneManager.Scenes scene) => OnSceneLoaded?.Invoke(scene);


}
