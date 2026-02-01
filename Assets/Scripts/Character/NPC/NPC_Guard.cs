using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class NPC_Guard : NPC
{
    public override void ChangeAlert()
    {
        base.ChangeAlert();

        if (isAlert) return;

        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, alertRadius);

        bool hasAlert = false;

        foreach (Collider2D collider in Colliders)
        {
            if (collider.GetComponentInParent<NPC_Infected>())
            {

                Debug.Log("检测到感染者");
                CurrentAlertValue += 100;
                hasAlert = true;
            }
            //获取行为
            if (collider.GetComponentInParent<Chara_Player>())
            {
                Debug.Log("检测到玩家");
                CurrentAlertValue += alertChangeSpeed * Time.deltaTime;
                hasAlert = true;
            }

        }

        if (!hasAlert)
        {
            CurrentAlertValue -= Time.deltaTime * alertChangeSpeed;
        }

    }

    public override void ChangeInfection(float infectionSpeed)
    {
        base.ChangeInfection(infectionSpeed);

        CurrentInfectionValue += infectionSpeed * Time.deltaTime / InfectionResistance;
    }

    protected override void AlertUpdate()
    {
        base.AlertUpdate();
        {
            Debug.Log("警戒状态");

            if (attackTarget != null)
            {
                visionAttach.SetLookAt(attackAimTarget);
                animControl.LookAt(attackAimTarget);
            }
            else
            {
                visionAttach.UnSetLookAt();
            }

        };
    }

    protected override void AttackUpdate()
    {
        base.AttackUpdate();
        if (attackTarget != null)
        {
            visionAttach.SetLookAt(attackAimTarget);
            animControl.LookAt(attackAimTarget);
        }
        else
        {
            visionAttach.UnSetLookAt();
        }

        if (attackTarget != null)
        {
            Weapon.AttackMode(attackTarget);
            //MoveTo();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
