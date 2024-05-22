using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents {
  public event Action OnTogglePillDropShadow;
  public void TogglePillDropShadow() => OnTogglePillDropShadow?.Invoke();

  public event Action<PlayerDetails> OnDetailsChanged;
  public void DetailsChanged(PlayerDetails details) => OnDetailsChanged?.Invoke(details);
}
