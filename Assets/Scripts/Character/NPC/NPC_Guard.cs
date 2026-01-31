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
        foreach (Collider2D collider in Colliders)
        {
            if (collider.GetComponent<NPC_Infected>())
            {

                Debug.Log("检测到感染者");
                CurrentAlertValue += 100;
            }
            //获取行为
            if(collider.GetComponent<Chara_Player>())
            {
                Debug.Log("检测到玩家");
                CurrentAlertValue += alertChangeSpeed * Time.deltaTime;
            }

        }

    }

    public override void ChangeInfection(float infectionSpeed)
    {
        base.ChangeInfection(infectionSpeed);

        CurrentInfectionValue += infectionSpeed * Time.deltaTime /(InfectionResistance + infectionSpeed * Time.deltaTime);
    }

    

    protected override void Update()
    {
        base.Update();
        ChangeInfection(0.01f);
    }
}
