using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundGridVisuals : MonoBehaviour {
  [SerializeField] Tilemap tilemap;
  [SerializeField] ColorData gameColors;

  [SerializeField] Tile lightTile;
  [SerializeField] Tile darkTile;

  public static BackgroundGridVisuals Instance { get; private set; }

  private void Awake() {
    DontDestroyOnLoad(gameObject);

    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

  private void Start() {
    SetBackground(GameSceneManager.Instance.ActiveScene);
    GameSceneManager.Instance.Events.OnSceneLoaded += SceneEvents_OnSceneLoaded;
  }

  private void OnDestroy() {
    GameSceneManager.Instance.Events.OnSceneLoaded -= SceneEvents_OnSceneLoaded;
  }

  private void SetBackground(GameSceneManager.Scenes scene) {
    switch (scene) {
      default:
      case GameSceneManager.Scenes.MainMenu:
        LoadMainMenuBackground();
        break;
      case GameSceneManager.Scenes.GameSetup:
        LoadGameSetupBackground();
        break;
      case GameSceneManager.Scenes.Gameplay:
        LoadGameplayBackground();
        break;
    }
  }

  private void SceneEvents_OnSceneLoaded(GameSceneManager.Scenes scene) {
    SetBackground(scene);
  }

  private void LoadMainMenuBackground() {
    SetColors(gameColors.lightBackgroundMainMenu, gameColors.darkBackgroundMainMenu);
  }

  private void LoadGameSetupBackground() {
    SetColors(gameColors.lightBackgroundSetupScreen, gameColors.darkBackgroundSetupScreen);
  }

  private void LoadGameplayBackground() {
    var playerSpeed = PlayerManager.Instance.Players[0]?.Details.dropSpeed;
    if (playerSpeed == null) { return; }

    SetColors(DropSpeedToLightColor(playerSpeed.Value), gameColors.darkBackgroundPlayScreen);
  }

  private void SetColors(Color lightColor, Color darkColor) {
    lightTile.color = lightColor;
    darkTile.color = darkColor;
    tilemap.RefreshAllTiles();
  }

  private Color DropSpeedToLightColor(DropSpeed speed) {
    switch(speed) {
      default:
      case DropSpeed.Low:
        return gameColors.lightBackgroundLow;
      case DropSpeed.Med:
        return gameColors.lightBackgroundMed;
      case DropSpeed.Hi:
        return gameColors.lightBackgroundHigh;
    }
  }
}
