using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusLevelDisplayBoxUI : MonoBehaviour {
  [HideInInspector] public int PlayerIndex = -1;
  [SerializeField] Text text;

  Player player;

  private void OnEnable() {
    if (PlayerIndex == -1) { return; }

    player = PlayerManager.Instance.Players[PlayerIndex];
    SetText(player.Details.virusLevel);

    player.Events.OnDetailsChanged += PlayerEvents_OnDetailsChanged;
  }

  private void OnDisable() {
    if (PlayerIndex == -1) { return; }

    //var player = PlayerManager.Instance.Players[PlayerIndex];
    //SetText(player.Details.virusLevel);
    player.Events.OnDetailsChanged -= PlayerEvents_OnDetailsChanged;
  }

  private void PlayerEvents_OnDetailsChanged(PlayerDetails details) {
    SetText(details.virusLevel);
  }

  private void SetText(int virusLevel) {
    text.text = virusLevel.ToString().PadLeft(2, '0');
  }
}
