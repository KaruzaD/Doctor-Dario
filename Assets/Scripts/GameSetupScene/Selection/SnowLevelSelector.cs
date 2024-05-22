using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLevelSelector : SelectorBase {
  public override int GetCurrentSelection(int playerIndex) {
    var player = PlayerManager.Instance.Players[playerIndex];
    var snowLevel = player.Details.snowLevel;
    return Array.IndexOf(Enum.GetValues(snowLevel.GetType()), snowLevel);
  }

  public override void SelectionHandler(int playerIndex, int selection) {
    var player = PlayerManager.Instance.Players[playerIndex];
    player.Details.snowLevel = (SnowLevel)(Enum.GetValues(player.Details.snowLevel.GetType())).GetValue(selection);

    player.DetailsChanged();
  }
}
