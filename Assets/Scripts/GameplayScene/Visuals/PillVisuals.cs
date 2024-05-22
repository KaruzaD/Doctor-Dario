using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PillVisuals : MonoBehaviour {
  [SerializeField] BlockVisuals leftOrTop;
  [SerializeField] BlockVisuals rightOrBottom;
  [SerializeField] public Transform PillTransform;
  [SerializeField] public BoxCollider2D Collider;

  [SerializeField] float moveDurationSeconds = 0.01f;
  [SerializeField] float dropInDurationSeconds = 0.1f;

  Animator animator;

  Tweener moveTween;

  Color leftOrTopColor { get => leftOrTop.Color; }
  Color rightOrBottomColor { get => rightOrBottom.Color; }

  Sequence horizontalRotateRightSequence;
  Sequence horizontalRotateLeftSequence;
  Sequence verticalRotateRightSequence;
  Sequence verticalRotateLeftSequence;

  public PillDirection Orientation;

  private Vector3 preRotatePosition;
  private Quaternion preRotateRotation;

  private void Awake() {
    SetupAnimationSequences();
  }

  private void OnDestroy() {
    if (moveTween != null) {
      moveTween.Complete();
      moveTween = null;
    }

    horizontalRotateRightSequence.Kill();
    horizontalRotateLeftSequence.Kill();
    verticalRotateRightSequence.Kill();
    verticalRotateLeftSequence.Kill();
  }

  public void Setup(Pill pill) {
    leftOrTop.SetColor(pill.middle.Color);
    leftOrTop.SetSprite(BlockSprite.left);
    rightOrBottom.SetColor(pill.right.Color);
    rightOrBottom.SetSprite(BlockSprite.right);
    Orientation = pill.orientation;
  }

  public void SetupPillShadow(Pill pill) {
    Setup(pill);
    leftOrTop.SetAlpha(0.2f);
    rightOrBottom.SetAlpha(0.2f);
  }

  public void DropIn(Vector3 from, TweenCallback onComplete = null) {
    PillTransform.DOMove(from, dropInDurationSeconds).From()
      .OnComplete(onComplete);
  }

  public void Rotate(Pill pill, bool clockwise) {
    Orientation = pill.orientation;
    
    if (clockwise) {
      RotateRight();
    } else {
      RotateLeft();
    }
  }

  public void Move(Vector3 destination, TweenCallback onComplete = null) {
    if (moveTween != null) {
      moveTween.Kill();
      moveTween = null;
    } 

    moveTween = PillTransform.DOLocalMove(destination, moveDurationSeconds);//.OnComplete(onComplete);

    //PillTransform.localPosition = destination;
    onComplete?.Invoke();
  }

  private void SetupAnimationSequences() {
    horizontalRotateRightSequence = DOTween.Sequence();
    horizontalRotateRightSequence.Append(transform.DOLocalMoveY(1, moveDurationSeconds));
    horizontalRotateRightSequence.Join(transform.DORotate(new Vector3(0, 0, -90), moveDurationSeconds)
      .OnComplete(() => {
        horizontalRotateRightSequence.Rewind();
        RotateRightAnimationFinishedTrigger();
      }));

    verticalRotateRightSequence = DOTween.Sequence();
    verticalRotateRightSequence.Append(transform.DORotate(new Vector3(0, 0, -90), moveDurationSeconds)
        .OnComplete(() => {
          verticalRotateRightSequence.Rewind();
          RotateRightAnimationFinishedTrigger();
        }));

    horizontalRotateLeftSequence = DOTween.Sequence();
    horizontalRotateLeftSequence.Append(transform.DORotate(new Vector3(0, 0, 90), moveDurationSeconds)
      .OnComplete(() => {
        horizontalRotateLeftSequence.Rewind();
        RotateLeftAnimationFinishedTrigger();
      }));

    verticalRotateLeftSequence = DOTween.Sequence();
    verticalRotateLeftSequence.Append(transform.DOLocalMoveX(1, moveDurationSeconds));
    verticalRotateLeftSequence.Join(transform.DORotate(new Vector3(0, 0, 90), moveDurationSeconds)
      .OnComplete(() => {
        verticalRotateLeftSequence.Rewind();
        RotateLeftAnimationFinishedTrigger();
      }));

  }

  public void RotateRight() {
    //use the horizontal rotation when vertical because the pill has already rotated in the data
    if (Orientation == PillDirection.vertical) {
      
      //horizontalRotateRightSequence.Play();
    } else {
      
      //verticalRotateRightSequence.Play();
    }

    RotateRightAnimationFinishedTrigger();
  }

  public void RotateLeft() {
    //use the horizontal rotation when vertical because the pill has already rotated in the data
    if (Orientation == PillDirection.vertical) {

      //horizontalRotateLeftSequence.Play();
    }
    else {
     
      //verticalRotateLeftSequence.Play();
    }

    RotateLeftAnimationFinishedTrigger();
  }

  private void RotateRightAnimationFinishedTrigger() {
    //use the horizontal rotation when vertical because the pill has already rotated in the data
    if (Orientation == PillDirection.vertical) {
      leftOrTop.transform.position += new Vector3(0, 1);
      rightOrBottom.transform.position += new Vector3(-1, 0);

      leftOrTop.SetSprite(BlockSprite.top);
      rightOrBottom.SetSprite(BlockSprite.bottom);
    }
    else {
      leftOrTop.transform.position += new Vector3(1, -1);

      SwapBlocks();
      leftOrTop.SetSprite(BlockSprite.left);
      rightOrBottom.SetSprite(BlockSprite.right);
    }
  }

  private void RotateLeftAnimationFinishedTrigger() {
    //use the horizontal rotation when vertical because the pill has already rotated in the data
    if (Orientation == PillDirection.vertical) {
      rightOrBottom.transform.position += new Vector3(-1, 1);

      SwapBlocks();
      leftOrTop.SetSprite(BlockSprite.top);
      rightOrBottom.SetSprite(BlockSprite.bottom);
    }
    else {
      leftOrTop.transform.position += new Vector3(0, -1);
      rightOrBottom.transform.position += new Vector3(1, 0);

      leftOrTop.SetSprite(BlockSprite.left);
      rightOrBottom.SetSprite(BlockSprite.right);
    }
  }

  private void SwapBlocks() {
    var temp = leftOrTop;
    leftOrTop = rightOrBottom;
    rightOrBottom = temp;
  }

  public void Remove() {
    Destroy(PillTransform.gameObject);
  }
}
