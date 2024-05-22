using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour {
  const string PLAYER_PREFS_MUSIC_VOLUME = "musicVolume";

  public static MusicManager Instance;

  [SerializeField] float fadeDurationSeconds = 0.2f;

  [SerializeField] private MusicClipsSO clips;

  private AudioSource audioSource;

  public MusicType SelectedGameplayMusicType = MusicType.Fever;
  public AudioClip SelectedGameplayMusic { get { 
    switch (SelectedGameplayMusicType) {
        default:
        case MusicType.Fever:
          return clips.fever;
        case MusicType.Chill:
          return clips.chill;
        case MusicType.Off:
          return null;
        case MusicType.Title:
          return clips.title;
      }
    } }

  float defaultVolume = .6f;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    audioSource = GetComponent<AudioSource>();
    var volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, defaultVolume);
    audioSource.volume = volume;
    audioSource.loop = true;
  }

  private void Start() {
    PlaySceneTrack();

    GameSceneManager.Instance.Events.OnSceneLoaded += Events_OnSceneLoaded;
    GameEvents.OnGameStarting += GameEvents_OnGameStarting;
    GameEvents.OnRoundOver += GameEvents_OnRoundOver;
  }

  private void GameEvents_OnRoundOver(bool won, int arg2) {
    if (!won) {
      var isSinglePlayer = PlayerManager.Instance.Players.Count == 1;

      if (isSinglePlayer) {
        PlayTrack(clips.gameOver);
      } else {
        PlayTrack(clips.vsGameOver);
      }
      return;
    }

    //won
    PlayTrack(GetClipForGameWon());
  }

  private void GameEvents_OnGameStarting(object sender, System.EventArgs e) {
    SwitchSceneTrack();
  }

  private void Events_OnSceneLoaded(GameSceneManager.Scenes scene) {
    if (scene != GameSceneManager.Scenes.Gameplay) {
      SwitchSceneTrack();
    }
  }

  private void OnDestroy() {
    GameSceneManager.Instance.Events.OnSceneLoaded -= Events_OnSceneLoaded;
    GameEvents.OnGameStarting -= GameEvents_OnGameStarting;
    GameEvents.OnRoundOver -= GameEvents_OnRoundOver;
  }

  public void ChangeVolume() {
    defaultVolume += .1f;
    if (defaultVolume > 1f) {
      defaultVolume = 0f;
    }
    audioSource.volume = defaultVolume;

    PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, defaultVolume);
    PlayerPrefs.Save();
  }

  public void PlayTrack(AudioClip clip, float volumeMultiplier = 1f) {
    if (clip == null) { 
      audioSource.Stop();
      return; }
    
    audioSource.clip = clip;
    audioSource.Play();

    audioSource.DOFade(0f, fadeDurationSeconds).From();
  }

  public void SwitchTrack(AudioClip clip) {
    if (clip == null) {
      audioSource.Stop(); 
      return;
    }
    
    StartCoroutine(SwitchTrackHelper(clip));
  }

  public IEnumerator SwitchTrackHelper(AudioClip clip) {
    yield return audioSource.DOFade(0f, fadeDurationSeconds).WaitForCompletion();
    audioSource.clip = clip;
    audioSource.Play();
    yield return audioSource.DOFade(defaultVolume, fadeDurationSeconds).WaitForCompletion();
  }

  private void PlaySceneTrack() {
    var track = GetClipForScene(GameSceneManager.Instance.ActiveScene);
    
    PlayTrack(track);
  }

  private void SwitchSceneTrack() {
    var track = GetClipForScene(GameSceneManager.Instance.ActiveScene);

    SwitchTrack(track);
  }

  private AudioClip GetClipForScene(GameSceneManager.Scenes scene) {
    switch(scene) {
      default:
      case GameSceneManager.Scenes.MainMenu:
        return clips.title;
      case GameSceneManager.Scenes.GameSetup:
        return clips.setup;
      case GameSceneManager.Scenes.Gameplay:
        return SelectedGameplayMusic;
    }
  }

  private AudioClip GetClipForGameWon() {
    switch (SelectedGameplayMusicType) {
      default:
      case MusicType.Fever:
        return clips.feverClear;
      case MusicType.Chill:
        return clips.chillClear;
      case MusicType.Off:
        return null;
    }
  }

  public float Volume => defaultVolume;

}

public enum MusicType {
  Off, Fever, Chill, Title
}

