using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpriteAttach : MonoBehaviour
{
    public Transform anchor;
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private void Awake()
    {
        if (spriteRenderers != null && spriteRenderers.Count > 0)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                spriteRenderer.sortingLayerName = "Scene";
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                spriteRenderer.sortingOrder = (int)(301f - anchor.position.y);
        }
    }
}
