using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/Music")]
public class MusicClipsSO : ScriptableObject {
  public AudioClip title;
  public AudioClip setup;
  public AudioClip fever;
  public AudioClip feverClear;
  public AudioClip chill;
  public AudioClip chillClear;
  public AudioClip gameOver;
  public AudioClip vsGameOver;
  public AudioClip level20Clear;
  public AudioClip endingTheme;
}
