using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour {
  [HideInInspector] public int PlayerIndex = -1;
  [HideInInspector] public float StepAmount;
  [HideInInspector] public Sprite Sprite;
  [HideInInspector] public Vector2 Bounds;
  [HideInInspector] public RectTransform ParentTransform;

  private int selectedLocation;
  [HideInInspector] public int SelectedLocation { get => selectedLocation; set {
      if (value < Bounds[0] || value > Bounds[1]) {
        return;
      }
      
      selectedLocation = value;
      SetLocation(value);
    } }

  [HideInInspector] public Action<int, int> SelectionHandler;

  RectTransform arrow;
  Image image;

  private void Awake() {
    arrow = GetComponent<RectTransform>();
    image = GetComponent<Image>();
  }

  private void OnEnable() {
    SetSprite();
    SetLocation(SelectedLocation);
  }

  private void OnDisable() {
    DisableInput();
  }

  public void EnableInput() {
    StartCoroutine(EnableInputDelayed());
  }

  private IEnumerator EnableInputDelayed() {
    if (PlayerIndex == -1) { yield break; }

    yield return new WaitForEndOfFrame();
    var player = PlayerManager.Instance.Players[PlayerIndex];
    player.Actions.UI.Navigate.performed += Navigate_performed;
  }

  public void DisableInput() {
    if (PlayerIndex == -1) { return; }
    
    try {
      var player = PlayerManager.Instance.Players[PlayerIndex];
      player.Actions.UI.Navigate.performed -= Navigate_performed;
    }
    catch (Exception e) {
      Debug.Log(e);
    }

  }

  private void Navigate_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
    var moveVec = context.ReadValue<Vector2>();
    var oldSelection = SelectedLocation;
    if (moveVec.x < 0) {
      SelectedLocation--;
    } else if (moveVec.x > 0) {
      SelectedLocation++;
    }

    if (SelectedLocation == oldSelection) {
      return;
    }

    SelectionHandler?.Invoke(PlayerIndex, SelectedLocation);

    if (moveVec != Vector2.zero) {
      SFXManager.Instance.PlaySound(SFXManager.Instance.Clips.menuHorizontal);
    }
  }

  public void SetSprite() { 
    image.sprite = Sprite;
  }

  public void SetLocation(int location) {
    if (arrow == null || ParentTransform == null) { return; }
    
    var locationX = (ParentTransform.rect.width * StepAmount) * location;
    arrow.anchoredPosition = new Vector2(locationX, arrow.anchoredPosition.y);
  }

  private void MoveRight() {
    var moveAmountX = ParentTransform.rect.width * StepAmount;
    arrow.anchoredPosition += new Vector2(moveAmountX, 0);
  }

  private void MoveLeft() {
    var moveAmountX = ParentTransform.rect.width * StepAmount;
    arrow.anchoredPosition -= new Vector2(moveAmountX, 0);
  }
}
