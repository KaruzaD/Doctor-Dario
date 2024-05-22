using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsListUI : MonoBehaviour {
  MenuItemUI[] menuItems;

  int currentSelection = 0;
  int CurrentSelection {
    get => currentSelection; 
    set {
      menuItems[currentSelection].IsSelected = false;
      currentSelection = Mathf.Abs(value) % menuItems.Length;
      menuItems[currentSelection].IsSelected = true;
    }
  }

  private void Start() {
    menuItems = GetComponentsInChildren<MenuItemUI>();

    if (menuItems.Length > 0) {
      menuItems[0].IsSelected = true;
    }

    foreach (var player in PlayerManager.Instance.Players.Values) {
      EnableInput(player);
    }

    PlayerManager.Instance.Events.OnPlayerJoined += Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft += Events_OnPlayerLeft;

  }

  private void OnDestroy() {
    foreach (var player in PlayerManager.Instance.Players.Values) {
      DisableInput(player);
    }

    PlayerManager.Instance.Events.OnPlayerJoined -= Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft -= Events_OnPlayerLeft;
  }

  private void Events_OnPlayerJoined(UnityEngine.InputSystem.PlayerInput playerInput, Player player) {
    EnableInput(player);
  }

  private void Events_OnPlayerLeft(UnityEngine.InputSystem.PlayerInput playerInput, Player player) {
    DisableInput(player);
  }

  private void Navigate_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
    var moveVector = context.ReadValue<Vector2>();
    if (moveVector.y > 0) {
      CurrentSelection--;
    } else if (moveVector.y < 0) {
      CurrentSelection++;
    }

    if (moveVector.y != 0) {
      SFXManager.Instance.PlaySound(SFXManager.Instance.Clips.menuVertiical);
    }
  }

  private void EnableInput(Player player) {
    player.Actions.UI.Navigate.performed += Navigate_performed;
  }

  private void DisableInput(Player player) {
    player.Actions.UI.Navigate.performed -= Navigate_performed;

  }
}
