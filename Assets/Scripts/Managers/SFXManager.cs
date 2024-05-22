using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour {
  public static SFXManager Instance;
  public SFXClipsSO Clips;

  private float volume = .8f;
  private AudioSource audioSource;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    audioSource = GetComponent<AudioSource>();
    audioSource.volume = volume;
  }

  private void Start() {
    GameEvents.OnTogglePause += GameEvents_OnTogglePause;
    GameEvents.OnGameStarted += GameEvents_OnGameStarted;
    GameEvents.OnGameOver += GameEvents_OnGameOver;
  }

  private void GameEvents_OnTogglePause() {
    PlaySound(Clips.gamePaused);
  }

  private void OnDestroy() {
    GameEvents.OnTogglePause -= GameEvents_OnTogglePause;
    GameEvents.OnGameStarted -= GameEvents_OnGameStarted;
    GameEvents.OnGameOver -= GameEvents_OnGameOver;
  }

  private void GameEvents_OnGameStarted(object sender, System.EventArgs e) {
    DisablePlayfieldEvents();
    EnablePlayfieldEvents();
  }


  private void PlayfieldEvents_OnSnowed(object sender, int playerIndex) {
    var playfield = sender as Playfield;
    AudioClip clip;
    if (playerIndex % 2 == 0) {
      clip = Clips.player1Snowed;
    } else {
      clip = Clips.player2Snowed;
    }

    PlaySound(clip);
  }

  private void PlayfieldEvents_OnBlocksRemoved(object sender, PlayfieldEvents.OnBlocksRemovedEventArgs e) {
    if (e.isCurrentPill || e.isNextPill) { return; }
    var playfield = sender as Playfield;
    if (playfield is null) { return; }

    AudioClip clip;
    if (playfield.ID % 2 == 0) {
      clip = Clips.player1BlocksCleared;
    } else {
      clip = Clips.player2BlocksCleared;
    }

    PlaySound(clip);
  }

  private void PlayfieldEvents_OnPillRotated(object sender, Pill pill, bool clockwise) {
    var playfield = sender as Playfield;
    PlaySound(Clips.pillRotated);
  }

  private void PlayfieldEvents_OnPillMoved(object sender, Pill pill, Vector2Int destination, bool isHorizontal) {
    if (!isHorizontal) { return; }
    var playfield = sender as Playfield;
    PlaySound(Clips.pillMoveHorizontal);
  }

  private void PlayfieldEvents_OnBlocksMoved(object sender, PlayfieldEvents.OnBlocksMovedEventArgs e) {
    if (e.movedBlocks is null || e.movedBlocks[0] is null) {
      return;
    }
    PlaySound(Clips.blocksFalling);
  }

  private void PlayfieldEvents_OnPillLanded(object sender, Pill pill) {
    var playfield = sender as Playfield;

    PlaySound(Clips.pillLanded);
  }

  private void GameEvents_OnGameOver(int playerID) {
    DisablePlayfieldEvents();
  }

  private void EnablePlayfieldEvents() {
    foreach (var playfield in PlayfieldManager.Instance.Playfields.Values) {
      playfield.Events.OnPillLanded += PlayfieldEvents_OnPillLanded;
      playfield.Events.OnBlocksMoved += PlayfieldEvents_OnBlocksMoved;
      playfield.Events.OnPillMoved += PlayfieldEvents_OnPillMoved;
      playfield.Events.OnPillRotated += PlayfieldEvents_OnPillRotated;
      playfield.Events.OnBlocksRemoved += PlayfieldEvents_OnBlocksRemoved;
      playfield.Events.OnSnowed += PlayfieldEvents_OnSnowed;
    }
  }

  public void DisablePlayfieldEvents() {
    foreach (var playfield in PlayfieldManager.Instance.Playfields.Values) {
      playfield.Events.OnPillLanded -= PlayfieldEvents_OnPillLanded;
      playfield.Events.OnBlocksMoved -= PlayfieldEvents_OnBlocksMoved;
      playfield.Events.OnPillMoved -= PlayfieldEvents_OnPillMoved;
      playfield.Events.OnPillRotated -= PlayfieldEvents_OnPillRotated;
      playfield.Events.OnBlocksRemoved -= PlayfieldEvents_OnBlocksRemoved;
      playfield.Events.OnSnowed -= PlayfieldEvents_OnSnowed;
    }
  }

  public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
    var cameraAdjustedPosition = Camera.main.transform.position;
    AudioSource.PlayClipAtPoint(audioClip, cameraAdjustedPosition, volumeMultiplier * volume);
    
  }

  public void PlaySound(AudioClip audioClip, float volumeMultiplier = 1f) {
    audioSource.PlayOneShot(audioClip, volumeMultiplier);
    Debug.Log($"{audioClip.name} played.");
    //PlaySound(audioClip, Camera.main.transform.position, volumeMultiplier);
  }

  public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
  }

  public void PlaySound(AudioClip[] audioClipArray, float volumeMultiplier = 1f) {
    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], volumeMultiplier);
  }
}
