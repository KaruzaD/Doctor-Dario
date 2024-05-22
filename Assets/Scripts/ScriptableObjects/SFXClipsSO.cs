using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/SFX")]
public class SFXClipsSO : ScriptableObject {
  public AudioClip blocksFalling;
  public AudioClip pillLanded;
  public AudioClip gamePaused;
  public AudioClip menuHorizontal;
  public AudioClip menuVertiical;

  public AudioClip pillMoveHorizontal;
  public AudioClip pillRotated;

  public AudioClip player1BlocksCleared;
  public AudioClip player2BlocksCleared;
  public AudioClip player3BlocksCleared;
  public AudioClip player4BlocksCleared;

  public AudioClip player1Snowed;
  public AudioClip player2Snowed;
  public AudioClip player3Snowed;
  public AudioClip player4Snowed;
}
