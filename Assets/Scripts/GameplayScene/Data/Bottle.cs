using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bottle : HasID {
  [SerializeField] private Playfield playfield;

  public BottleEvents Events { get; } = new BottleEvents();

  private void Start() {

  }



  private void OnDestroy() {

  }

}

public class BottleEvents {
  public event Action<Pill> OnNextPillGenerated;
  public void NextPillGenerated(Pill pill) => OnNextPillGenerated?.Invoke(pill);
  
  public event Action<Pill> OnCurrentPillDropped;
  public void CurrentPillDropped(Pill pill) => OnCurrentPillDropped?.Invoke(pill);
}
