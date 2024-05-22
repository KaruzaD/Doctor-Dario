using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfieldUI : MonoBehaviour {
  [SerializeField] Playfield playfield;

  [SerializeField] Text virusCount;
  [SerializeField] Text levelNumber;
  [SerializeField] Text dropSpeed;
  [SerializeField] Text playerNumber;

  private void Start() {
    SetupTexts();
    playfield.Events.OnVirusCountUpdated += PlayfieldEvents_OnVirusCountUpdated;
  }

  private void OnDestroy() {
    playfield.Events.OnVirusCountUpdated -= PlayfieldEvents_OnVirusCountUpdated;
  }

  private void SetupTexts() {
    levelNumber.text = playfield.Player.Details.virusLevel.ToString().PadLeft(2, '0');
    dropSpeed.text = playfield.Player.Details.dropSpeed.ToString();
    playerNumber.text = $"{playfield.Player.ID + 1}P";
  }

  private void PlayfieldEvents_OnVirusCountUpdated(int virusesRemaining) {
    var countText = virusesRemaining.ToString().PadLeft(2, '0');
    virusCount.text = countText;
  }
}
