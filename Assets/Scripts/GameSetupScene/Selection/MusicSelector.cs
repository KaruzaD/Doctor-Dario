using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelector : SelectorBase {
  public override int GetCurrentSelection(int playerIndex) {
    var music = MusicManager.Instance.SelectedGameplayMusicType;
    return Array.IndexOf(Enum.GetValues(music.GetType()), music);
  }

  public override void SelectionHandler(int playerIndex, int selection) {
    MusicManager.Instance.SelectedGameplayMusicType = (MusicType)Enum.GetValues(typeof(MusicType)).GetValue(selection);
  }
}
