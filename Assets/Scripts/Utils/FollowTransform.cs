using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour {
  Transform targetTransform;

  public Transform TargetTransform { set => targetTransform = value; }

  private void LateUpdate() {
    if (targetTransform == null) { return; }
    transform.position = targetTransform.position;
    transform.rotation = targetTransform.rotation;
  }
}
