using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasInfectedAttach : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.DOFade(0.2f, 0.5f));
        sequence.Append(spriteRenderer.DOFade(1f, 0.5f));
        sequence.SetLink(gameObject);
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.Play();
    }
    private void OnDisable()
    {
        DOTween.Kill(gameObject);
    }
}
