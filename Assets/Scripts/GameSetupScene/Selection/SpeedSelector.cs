using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSelector : SelectorBase {
  public override int GetCurrentSelection(int playerIndex) {
    var player = PlayerManager.Instance.Players[playerIndex];
    var speed = player.Details.dropSpeed;
    return Array.IndexOf(Enum.GetValues(speed.GetType()), speed);
  }

  public override void SelectionHandler(int playerIndex, int selection) {
    var player = PlayerManager.Instance.Players[playerIndex];
    player.Details.dropSpeed = (DropSpeed)(Enum.GetValues(player.Details.dropSpeed.GetType())).GetValue(selection);

    player.DetailsChanged();
  }
}
