using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingTxtUI : MonoBehaviour {
  [SerializeField] float flashRateSeconds = 0.5f;

  Sequence flashSequence;

  Text text;

  private void Awake() {
    text = GetComponent<Text>();

    float halfFlashRate = flashRateSeconds * 0.5f;

    flashSequence = DOTween.Sequence();
    flashSequence.Append(text.DOFade(0, halfFlashRate));
    flashSequence.Append(text.DOFade(1, halfFlashRate));
    flashSequence.SetLoops(-1);
  }

  private void OnEnable() {
    StartLoop();
  }

  private void OnDisable() {
    StopLoop();
  }

  private void OnDestroy() {
    flashSequence.Kill();
  }

  void StartLoop() {
    flashSequence.Play();
  }

  void StopLoop() {
    flashSequence.Pause();
  }
}
