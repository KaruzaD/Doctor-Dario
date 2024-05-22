using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemUI : MonoBehaviour {
  [SerializeField] Transform selectionOutline;
  [SerializeField] SelectorBase selector;

  private bool isSelected = false;
  public bool IsSelected { get => isSelected; set {
      isSelected = value;

      if (isSelected) {
        selectionOutline.gameObject.SetActive(true);
        selector.IsSelected = true;
        selector?.EnableAllInputs();
      } else {
        selectionOutline.gameObject.SetActive(false);
        selector.IsSelected = false;
        selector?.DisableAllInputs();
      }
    } }

  private void Awake() {
    selectionOutline.gameObject.SetActive(false);
  }
}
