using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyBox : MonoBehaviour {
  [SerializeField] GameObject checkMark;
  [SerializeField] GameObject crossOut;

  private void Awake() {
    Clear();
  }

  public void Check() {
    checkMark.SetActive(true);
    crossOut.SetActive(false);
  }

  public void CrossOUt() {
    checkMark.SetActive(false);
    crossOut.SetActive(true);
  }

  public void Clear() {
    checkMark.SetActive(false);
    crossOut.SetActive(false);
  }
}
