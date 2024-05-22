using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundOverUI : MonoBehaviour {
  [SerializeField] GameObject RoundOverContainer;
  [SerializeField] GameObject CheckMark;
  [SerializeField] GameObject CrossOut;

  [SerializeField] float fadeInDurationSeconds;

  [SerializeField] Playfield playfield;

  Tween fadeTween;
  CanvasGroup group;

  private void Awake() {
    group = GetComponent<CanvasGroup>();
  }

  private void Start() {
    GameEvents.OnRoundOver += GameEvents_OnRoundOver;
    GameEvents.OnGameStarting += GameEvents_OnGameStarting;

    transform.localPosition = new Vector3(playfield.Width / 2.0f - 0.5f, (playfield.Height) * -1);

    Clear();
  }

  private void OnDestroy() {
    GameEvents.OnRoundOver -= GameEvents_OnRoundOver;
    GameEvents.OnGameStarting -= GameEvents_OnGameStarting;

    fadeTween.Kill();
  }

  private void GameEvents_OnGameStarting(object sender, System.EventArgs e) {
    Clear();
  }

  private void GameEvents_OnRoundOver(bool hasWon, int playerIndex) {
    if (playfield.ID != playerIndex) {
      return;
    }

    Enable();

    if (hasWon) {
      CheckMark.SetActive(true);
    } else {
      CrossOut.SetActive(true);
    }
  }

  private void Enable() {
    RoundOverContainer.SetActive(true);

    group.alpha = 0.0f;
    fadeTween = group.DOFade(1.0f, fadeInDurationSeconds);
  }

  private void Clear() {
    CheckMark.SetActive(false);
    CrossOut.SetActive(false);
    RoundOverContainer.SetActive(false);
  }
}
