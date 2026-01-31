using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSpriteAttach : MonoBehaviour
{
    public Transform anchor;
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        if (spriteRenderers != null && spriteRenderers.Count > 0)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                spriteRenderer.sortingLayerName = "Scene";
        }

    }
    void Update()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            spriteRenderer.sortingOrder = (int)(300f - anchor.position.y);
    }


}
