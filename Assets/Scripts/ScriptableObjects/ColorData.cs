using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
public class ColorData : ScriptableObject {
  public Color lightBackgroundLow;
  public Color lightBackgroundMed;
  public Color lightBackgroundHigh;
  public Color darkBackgroundPlayScreen;

  public Color lightBackgroundMainMenu;
  public Color darkBackgroundMainMenu;

  public Color lightBackgroundSetupScreen;
  public Color darkBackgroundSetupScreen;

  public Color bottleGlass;
  public Color bottleGlassLight;

  public Color dialogBoxBackground;
  public Color dialogBoxBorder;
}