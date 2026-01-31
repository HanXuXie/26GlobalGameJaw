using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl_Chara : MonoBehaviour
{
    public Transform Transform_LookAt;

    private SpriteRenderer render;
    private Animator animator;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    #region 对外接口
    public void Flip(bool _ifFlip)
    {
        render.flipX = _ifFlip;
    }

    public void LookAt(Transform _target)
    {
        Transform_LookAt = _target;
    }

    public void FreeLook()
    {
        Transform_LookAt = null;
    }
    #endregion
}
