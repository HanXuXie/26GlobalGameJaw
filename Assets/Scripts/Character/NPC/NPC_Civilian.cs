using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class NPC_Civilian : NPC
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
                attackTarget = collider.GetComponentInParent<NPC_Infected>();

                AttackTargetUpdate();

                hasAlert = true;
            }
            //获取玩家行为
            if (collider.GetComponentInParent<Chara_Player>() && collider.GetComponentInParent<Chara_Player>().PlayerState != PlayerState.Henshin)
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

        CurrentInfectionValue += infectionSpeed * Time.deltaTime / (InfectionResistance + infectionSpeed * Time.deltaTime);

        if (CurrentInfectionValue > maxInfectionValue)
        {
            CurrentInfectionValue = maxInfectionValue;
        }

    }

    protected override void AlertUpdate()
    {
        base.AlertUpdate();

        if (attackTarget == null) return;

        Vector3 distance = transform.position - attackTarget.transform.position;



        if (canSet)
        {
            StartCoroutine(SetMovePoint());
            MoveTo((MoveSpeed * distance.normalized) + transform.position);
        }

    }

    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if (attackTarget == null) return;
        Vector3 distance = transform.position - attackTarget.transform.position;
        if (canSet)
        {
            StartCoroutine(SetMovePoint());
            MoveTo((MoveSpeed * distance.normalized) + transform.position);
        }

    }

    protected override void Update()
    {
        base.Update();

        AttackTargetUpdate();
    }
}
