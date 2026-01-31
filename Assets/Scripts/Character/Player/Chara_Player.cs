using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MulInputSystem;

public class Chara_Player : CharaBase
{
    public Vector3 Speed;


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
    }

    public void FixedUpdate()
    {
        // 更新速度
        rb.velocity = Speed;
    }

    // 普A
    public void Attack()
    {

    }

    // 技能
    public void Skill()
    {


    }
    // 引爆
    public void Boom()
    {

    }
}
