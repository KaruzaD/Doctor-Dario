using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextUI : MonoBehaviour {
  [SerializeField] private float effectStrength;
  [SerializeField] private float durationSeconds;
  [SerializeField] private float intervalSeconds;
  
  Outline outline;
  Sequence outlineStrobe;
  float effectStart;

  private void Awake() {
    outline = GetComponent<Outline>();

    effectStart = outline.effectDistance.x;

    outlineStrobe = DOTween.Sequence();

    outlineStrobe.Append(DOTween.To(
      () => outline.effectDistance,
      x => outline.effectDistance = x,
      new Vector2(effectStart + effectStrength, effectStart + effectStrength),
      durationSeconds
      ));

    outlineStrobe.AppendInterval(intervalSeconds);

    outlineStrobe.Append(DOTween.To(
      () => outline.effectDistance,
      x => outline.effectDistance = x,
      new Vector2(effectStart, effectStart),
      durationSeconds
      ));

    outlineStrobe.AppendInterval(intervalSeconds);

    outlineStrobe.SetLoops(-1);
    outlineStrobe.Play();
  }

  private void OnDestroy() {
    outlineStrobe.Kill();
  }
}
