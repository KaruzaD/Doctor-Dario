using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneLoader : MonoBehaviour {
  [SerializeField] Transform gameManager;
  [SerializeField] Transform playfieldManager;
  [SerializeField] Transform scoreManager;

  void Awake() {
    if (GameManager.Instance == null) Instantiate(gameManager, transform);
    if (PlayfieldManager.Instance == null) Instantiate(playfieldManager, transform);
    if (ScoreManager.Instance == null) Instantiate(scoreManager, transform);
  }
}
