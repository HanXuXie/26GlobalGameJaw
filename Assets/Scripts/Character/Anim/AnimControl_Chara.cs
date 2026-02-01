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

    private void Update()
    {
        if (Transform_LookAt != null)
        {
            if(Mathf.Abs(Transform_LookAt.position.x - transform.position.x)> 0.05f)
            {
                Flip(Transform_LookAt.position.x < transform.position.x);
            }
        }
    }

    #region 对外接口
    public void Flip(bool _ifFlip)
    {
        render.flipX = _ifFlip;
    }

    public void onWalk(bool _onWalk)
    {
        animator.SetBool("onWalk", _onWalk);
    }

    public void onHenshin(bool _onHenshin)
    {
        animator.SetBool("onHenshin", _onHenshin);
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
