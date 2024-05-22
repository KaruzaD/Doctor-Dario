using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusLevelSelector : SelectorBase {
  public override int GetCurrentSelection(int playerIndex) {
    var player = PlayerManager.Instance.Players[playerIndex];
    return player.Details.virusLevel;
  }

  public override void SelectionHandler(int playerIndex, int selection) {
    var player = PlayerManager.Instance.Players[playerIndex];
    var virusLevel = player.Details.virusLevel;

    player.Details.virusLevel = selection;
    player.DetailsChanged();
  }
}
