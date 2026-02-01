using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

                AppMain.instance.FindInfector();

                hasAlert = true;
            }
            //获取行为
            if (collider.GetComponentInParent<Chara_Player>() && collider.GetComponentInParent<Chara_Player>().PlayerState == PlayerState.Henshin)
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

        if (CurrentInfectionValue > maxInfectionValue)
        {
            CurrentInfectionValue = maxInfectionValue;
        }
    }

    protected override void AlertUpdate()
    {
        base.AlertUpdate();
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

            Debug.Log("警卫使用武器");

            Battle = Weapon.AttackMode(attackTarget);

            if (Battle) return;

            if (canSet && Vector3.Distance(attackAimTarget.position, transform.position) > Weapon.weaponRange - 0.5)
            {
                StartCoroutine(SetMovePoint());
                MoveTo(TargetAcquisition.HalfRoad(this.transform.position, attackTarget.transform.position));
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }

}
