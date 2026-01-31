using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MulInputSystem;
using Sirenix.OdinInspector;

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

        // 右键移动
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            MoveTo(worldPos);
        }

        // 左键普攻
        if(Input.GetMouseButtonUp(0))
        {
            DoAttack();
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

    // 普A
    public void DoAttack()
    {

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
