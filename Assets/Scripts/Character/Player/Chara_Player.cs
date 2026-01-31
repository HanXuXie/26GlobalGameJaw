using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MulInputSystem;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum PlayerState
{
    [LabelText("普通状态")]
    Normal,

    [LabelText("疫医状态")]
    Henshin,
}

public class Chara_Player : CharaBase
{
    public Vector3 Speed;
    public PlayerState PlayerState;

    public SpriteRenderer Sprite_AttackRange;

    protected override void Update()
    {
        base.Update();

        Update_Input();
    }

    public void Update_Input()
    {
        var axis_Horizonta = MulInputSystem.MulInputSystem.GetInput_AxisRaw(PlayerTag.Player1, InputType.Axis_Horizonta);
        var axis_Vertical = MulInputSystem.MulInputSystem.GetInput_AxisRaw(PlayerTag.Player1, InputType.Axis_Vertical);

        Speed.x = axis_Horizonta * MoveSpeed;
        Speed.y = axis_Vertical * MoveSpeed;

        if(Mathf.Abs(Speed.x) > 0.05f)
        {
            animControl.Flip(Speed.x < 0);
        }

        // 右键移动
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            MoveTo(worldPos);
        }

        // 左键普攻
        if(Input.GetMouseButtonDown(0) && !onAttack)
        {
            DoAttack();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndAttack();
        }

        // 空格爆发
        if (MulInputSystem.MulInputSystem.GetInput_Button(PlayerTag.Player1, InputType.Tap_Space))
        {
            DoBoom();
        }
    }

    public void FixedUpdate()
    {
        // 更新速度
        rb.velocity = Speed;
    }

    private bool onAttack;
    // 开始普A
    public void DoAttack()
    {
        Sprite_AttackRange.DOComplete();

        onAttack = true;
        PlayerState = PlayerState.Henshin;
        animControl.onHenshin(true);

        Sprite_AttackRange.color = new Color(1,1,1,0);
        Sprite_AttackRange.gameObject.SetActive(true);
        Sprite_AttackRange.DOFade(1, 1)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Sprite_AttackRange.color = new Color(1, 1, 1, 1);
                onAttack = false;
            })
            .SetLink(Sprite_AttackRange.gameObject);
    }


    // 停止普A
    public void EndAttack()
    {
        Sprite_AttackRange.DOComplete();
        onAttack = true;

        Sprite_AttackRange.color = new Color(1, 1, 1, 1);
        Sprite_AttackRange.DOFade(0, 1)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Sprite_AttackRange.color = new Color(1, 1, 1, 0);
                Sprite_AttackRange.gameObject.SetActive(false);

                PlayerState = PlayerState.Normal;
                animControl.onHenshin(false);
                onAttack = false;
            })
            .SetLink(Sprite_AttackRange.gameObject);
    }

    // 技能
    public void DoSkill()
    {


    }

    // 引爆
    public void DoBoom()
    {

    }
}
