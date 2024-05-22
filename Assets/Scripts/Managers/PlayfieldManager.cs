using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldManager : MonoBehaviour {
  public static PlayfieldManager Instance {get; private set;}
  
  [SerializeField] Transform playfieldPrefab;

  public Dictionary<int, Playfield> Playfields;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    Playfields = new Dictionary<int, Playfield>();
    PlayerManager.Instance.Events.OnPlayerJoined += Events_OnPlayerJoined;
  }

  private void OnDestroy() {
    PlayerManager.Instance.Events.OnPlayerJoined -= Events_OnPlayerJoined;

  }

  private void Events_OnPlayerJoined(UnityEngine.InputSystem.PlayerInput playerInput, Player player) {
    Add(player);
  }

  public void Add(Player player) {
    var playfieldTransform = Instantiate(playfieldPrefab, transform);
    var playfield = playfieldTransform.GetComponent<Playfield>();
    playfield.Player = player;
    player.Playfield = playfield;

    playfield.SetupStates();
    
    Playfields.Add(player.ID, playfield);
    ResetPositions();
  }

  public void Remove(int id) {
    var playfield = Playfields.GetValueOrDefault(id);
    if (playfield == null) return;

    Playfields.Remove(id);
    Destroy(playfield.gameObject);

    ResetPositions();
  }

  public void ResetPositions() {
    var camera = Camera.main;
    var screenSection = (Screen.width / (float)(Playfields.Count + 1));

    for (int i = 0; i < Playfields.Count; i++) {
      var playfield = Playfields[i];

      var screenX = screenSection * (i+1);
      var screenPoint = camera.WorldToScreenPoint(playfield.StartPosition);
      var worldPoint = camera.ScreenToWorldPoint(new Vector3(screenX, screenPoint.y, screenPoint.z));

      playfield.SetPosition(worldPoint);
    }
  }

  public void SnowRandomPlayer(int callingPlayerId, List<BlockColor> colorsToSnow) {
    if (Playfields.Count <= 1) {
      return;
    }
    
    Playfield playfieldToSnow = null;

    while (playfieldToSnow is null) {
      int randomID = UnityEngine.Random.Range(0, Playfields.Count);
      if (randomID == callingPlayerId) {
        continue;
      }

      playfieldToSnow = Playfields[randomID];
    }

    playfieldToSnow.Snowed(new List<BlockColor>(colorsToSnow));
  }
}
