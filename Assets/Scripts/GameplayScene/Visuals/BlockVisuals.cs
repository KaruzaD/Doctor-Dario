using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using DG.Tweening;

public class BlockVisuals : MonoBehaviour {
  [SerializeField] Transform blockTransform;
  
  [SerializeField] Sprite leftBlockSprite;
  [SerializeField] Sprite rightBlockSprite;
  [SerializeField] Sprite leftBlockReflectionSprite;
  [SerializeField] Sprite rightBlockReflectionSprite;

  [SerializeField] Sprite unattachedBlockSprite;
  [SerializeField] Sprite unattachedReflectionSprite;

  [SerializeField] Sprite[] virusSprite;

  [SerializeField] Sprite destroyedBlockSprite;

  [SerializeField] Color yellow;
  [SerializeField] Color blue;
  [SerializeField] Color red;

  [SerializeField] Transform highlightTransform;
  [SerializeField] SpriteRenderer highlightRenderer;

  [SerializeField] float AnimationDurationSeconds = 0.01f;

  [HideInInspector] public SpriteRenderer Renderer;

  public Color Color { get => Renderer.color; }

  public Block Block;

  private Tween moveTween;

  public void Awake() {
    Renderer = GetComponent<SpriteRenderer>();
  }

  private void OnDestroy() {
    if (moveTween != null) {
      moveTween.Complete();
      moveTween = null;
    }
  }

  public void Setup(Block block) {
    Block = block;
    SetColor(block.Color);
    ResetSprite(block);
  }

  public void ResetSprite(Block block) {
    if (block is Virus) {
      SetSprite(BlockSprite.virus);
    }
    else if (!block.HasPartner) {
      SetSprite(BlockSprite.unattached);
    }
    else if (block.IsPartnerRight) {
      SetSprite(BlockSprite.left);
    }
    else if (block.IsPartnerLeft) {
      SetSprite(BlockSprite.right);
    }
    else if (block.IsPartnerAbove) {
      SetSprite(BlockSprite.bottom);
    }
    else {
      SetSprite(BlockSprite.top);
    }
  }

  public void Remove() {
    if (Block.HasPartner) {
      var partnerObject = Block.Partner.SpawnedObject;
      var partnerVisuals = partnerObject.GetComponentInChildren<BlockVisuals>();
      partnerVisuals.SetSprite(BlockSprite.unattached);
    }

    SetSprite(BlockSprite.destroyed);

    Renderer.DOFade(0.0f, 0.3f).OnComplete(() => {
      Block.SpawnedObject = null;
      Destroy(blockTransform.gameObject);
    });
  }

  public void Move(Vector3 destination) {
    if (moveTween != null) {
      moveTween.Kill();
      moveTween = null;
    }
    moveTween = blockTransform.DOLocalMove(destination, AnimationDurationSeconds);
    //blockTransform.localPosition = destination;
  }

  public void SetColor(BlockColor color) {
    switch (color) {
      case BlockColor.yellow:
        Renderer.color = yellow;
        highlightRenderer.color = blue;
        break;
      case BlockColor.blue:
        Renderer.color = blue;
        highlightRenderer.color = yellow;
        break;
      default:
      case BlockColor.red:
        Renderer.color = red;
        highlightRenderer.color = yellow;
        break;
    }
  }

  public void SetSprite(BlockSprite sprite) {
    Renderer.flipY = false;
    highlightRenderer.flipY = false;
    Renderer.flipX = false;
    highlightRenderer.flipX = false;
    transform.rotation = Quaternion.identity;
    highlightTransform.rotation = Quaternion.identity;

    switch (sprite) {
      default:
      case BlockSprite.left:
        Renderer.sprite = leftBlockSprite;
        highlightRenderer.sprite = leftBlockReflectionSprite;
        break;
      case BlockSprite.right:
        Renderer.sprite = rightBlockSprite;
        highlightRenderer.sprite = rightBlockReflectionSprite;
        break;
      case BlockSprite.top:
        Renderer.sprite = leftBlockSprite;
        highlightRenderer.sprite = leftBlockReflectionSprite;
        var topRotation = new Vector3(0, 0, -90);
        transform.Rotate(topRotation);
        Renderer.flipY = true;
        highlightRenderer.flipY = true;
        break;
      case BlockSprite.bottom:
        Renderer.sprite = rightBlockSprite;
        highlightRenderer.sprite = rightBlockReflectionSprite;
        var bottomRotation = new Vector3(0, 0, -90);
        transform.Rotate(bottomRotation);
        Renderer.flipY = true;
        highlightRenderer.flipY = true;
        break;
      case BlockSprite.unattached:
        Renderer.sprite = unattachedBlockSprite;
        highlightRenderer.sprite = unattachedReflectionSprite;
        break;
      case BlockSprite.destroyed:
        Renderer.sprite = destroyedBlockSprite;
        highlightRenderer.sprite = null;
        break;
      case BlockSprite.virus:
        Renderer.sprite = virusSprite[0];
        highlightRenderer.sprite = null;
        break;
    }
  }

  public void SetAlpha(float alpha) {
    Renderer.DOFade(alpha, 0);
    highlightRenderer.DOFade(alpha, 0);
  }

}

public enum BlockSprite {
  left,
  right,
  top, 
  bottom,
  unattached,
  destroyed,
  virus,
}
