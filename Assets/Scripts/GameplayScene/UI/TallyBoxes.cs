using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TallyBoxes : MonoBehaviour {
  [SerializeField] TallyBox[] checkBoxes;
  [SerializeField] TallyBox[] crossBoxes;

  [SerializeField] Playfield playfield;

  int totalChecks = 0;
  int totalCrosses = 0;

  private void Start() {
    GameEvents.OnGameInit += GameEvents_OnGameInit;
    GameEvents.OnRoundOver += GameEvents_OnRoundOver;
  }

  private void OnDestroy() {
    GameEvents.OnGameInit -= GameEvents_OnGameInit;
    GameEvents.OnRoundOver -= GameEvents_OnRoundOver;
  }

  private void GameEvents_OnRoundOver(bool hasWon, int playerId) {
    if (playerId != playfield.ID) {
      return;
    }

    if (hasWon) {
      AddCheck();
    } else {
      AddCrossOut();
    }
  }


  private void GameEvents_OnGameInit(object sender, System.EventArgs e) {
    ResetTallies();
  }

  private void ResetTallies() {
    totalChecks = 0;
    totalCrosses = 0;
    foreach (var box in checkBoxes) { box.Clear(); }
  }

  private void AddCheck() {
    if (totalChecks >= checkBoxes.Length) { return; }
    checkBoxes[totalChecks++].Check();
  }

  private void AddCrossOut() {
    if (totalCrosses >= crossBoxes.Length) { return; }
    crossBoxes[totalCrosses++].CrossOUt();
  }
}
