using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class SelectorBase : MonoBehaviour {
  [SerializeField] float selectionAmount;
  [SerializeField] Vector2Int bounds;
  
  [SerializeField] Sprite selectionArrowSprite;

  SelectionArrow[] selectionArrows;
  RectTransform rectTransform;

  [HideInInspector] public bool IsSelected = false;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();

    selectionArrows = GetComponentsInChildren<SelectionArrow>();

    foreach(var arrow in selectionArrows) {
      arrow.PlayerIndex = -1;
      arrow.gameObject.SetActive(false);
    }
  }

  private void Start() {
    PlayerManager.Instance.Events.OnPlayerJoined += Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft += Events_OnPlayerLeft;

    for (int i = 0; i < PlayerManager.Instance.Players.Count; i++) {
      EnableSelectionArrow(i);
    }
  }

  private void OnDisable() {
    PlayerManager.Instance.Events.OnPlayerJoined -= Events_OnPlayerJoined;
    PlayerManager.Instance.Events.OnPlayerLeft -= Events_OnPlayerLeft;

    DisableAllInputs();
  }

  private void Events_OnPlayerJoined(UnityEngine.InputSystem.PlayerInput playerInput, Player player) {
    if (playerInput.playerIndex >= selectionArrows.Length) { return; }
    
    EnableSelectionArrow(playerInput.playerIndex);
  }

  private void EnableSelectionArrow(int index) {
    if (index >= selectionArrows.Length) { return; }

    var arrow = selectionArrows[index];
    arrow.PlayerIndex = index;
    arrow.StepAmount = 1 / (selectionAmount-1);
    arrow.Sprite = selectionArrowSprite;
    arrow.Bounds = bounds;
    arrow.ParentTransform = rectTransform;
    arrow.SelectionHandler = SelectionHandler;
    arrow.SelectedLocation = GetCurrentSelection(index);

    arrow.gameObject.SetActive(true);
    if (IsSelected) { arrow.EnableInput(); }
  }

  private void Events_OnPlayerLeft(UnityEngine.InputSystem.PlayerInput playerInput, Player player) {
    if (playerInput.playerIndex >= selectionArrows.Length) { return; }

    selectionArrows[playerInput.playerIndex].gameObject.SetActive(false);
  }


  public abstract void SelectionHandler(int playerIndex, int selection);
  public abstract int GetCurrentSelection(int playerIndex);

  public void EnableAllInputs() {
    foreach (var arrow in selectionArrows) {
      if (!arrow.isActiveAndEnabled) {
        continue;
      }
      arrow.EnableInput();
    }
  }

  public void DisableAllInputs() {
    foreach (var arrow in selectionArrows) {
      if (!arrow.isActiveAndEnabled) {
        continue;
      }
      arrow.DisableInput();
    }
  }
}
