using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerManager : MonoBehaviour {
  public static PlayerManager Instance { get; private set; }

  public PlayerInputManager inputManager;
   
  public PlayerManagerEvents Events { get; private set; } = new PlayerManagerEvents();


   private void Awake() {
     if (Instance != null && Instance != this) {
       Destroy(this);
     }
     else {
       Instance = this;
     }

     inputManager = GetComponent<PlayerInputManager>();
  }

  void OnPlayerJoined(PlayerInput playerInput) {
    Debug.Log("Player joined with ID: " + playerInput.playerIndex);
    var player = playerInput.gameObject.GetComponent<Player>();
    player.ID = playerInput.playerIndex;
    player.Actions = new GameInputActions();
    player.Actions.devices = playerInput.devices;

    //playerInput.uiInputModule = gameSetupInputModule;

    Add(player);

    Events.PlayerJoined(playerInput, player);
  }

  void OnPlayerLeft(PlayerInput playerInput) {
    Debug.Log("Player left with ID: " + playerInput.playerIndex);
    var player = playerInput.gameObject.GetComponent<Player>();

    Events.PlayerLeft(playerInput, player);
    //Remove(player.ID);
  }

  public Dictionary<int, Player> Players { get; private set; } = new Dictionary<int, Player>();

  public  void Add(Player player) {
    Players.Add(player.ID, player);
  }

  public void Remove(int id) {
    if (Players.TryGetValue(id, out var player)) {
      Destroy(player.gameObject);
      Players.Remove(id);
      
    }
  }

  public void HandleAllPlayerUpdates() {
    foreach (var player in Players.Values) {
      player.HandleUpdate();
    }
  }

  public Playfield GetPlayfield(int id) => Players[id].Playfield;
  public PlayerEvents GetEvents(int id) => Players[id].Events;
  public PlayfieldEvents GetPlayfieldEvents(int id) => GetPlayfield(id).Events;
  
}

public class PlayerManagerEvents {
  public event Action<PlayerInput, Player> OnPlayerJoined;
  public void PlayerJoined(PlayerInput playerInput, Player player) => OnPlayerJoined?.Invoke(playerInput, player);

  public event Action<PlayerInput, Player> OnPlayerLeft;
  public void PlayerLeft(PlayerInput playerInput, Player player) => OnPlayerLeft?.Invoke(playerInput, player);
}
