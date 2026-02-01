using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Infected : NPC
{
    public override void ChangeAlert()
    {
        base.ChangeAlert();

        if (isAlert) return;
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, alertRadius);


        bool hasAlert = false;

        foreach (Collider2D collider in Colliders)
        {
            Debug.Log(collider);

            if (collider.GetComponentInParent<NPC_Guard>())
            {

                Debug.Log("检测到警卫");
                CurrentAlertValue += 100;

                visionAttach.SetLookAt(collider.transform);
                animControl.LookAt(collider.transform);

                hasAlert = true;
            }
            if (collider.GetComponentInParent<NPC_Civilian>())
            {
                Debug.Log("检测到良民");
                CurrentAlertValue += 100;

                visionAttach.SetLookAt(collider.transform);
                animControl.LookAt(collider.transform);

                hasAlert = true;
            }

        }

        if (!hasAlert)
        {
            CurrentAlertValue -= Time.deltaTime * alertChangeSpeed;
        }

    }

    protected override void AlertUpdate()
    {
        base.AlertUpdate();
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


            Battle = Weapon.AttackMode(attackTarget);

            if (Battle) return;

            if (canSet && Vector3.Distance(attackAimTarget.position, transform.position) > Weapon.weaponRange)
            {
                StartCoroutine(SetMovePoint());
                MoveTo(TargetAcquisition.HalfRoad(this.transform.position, attackTarget.transform.position));
            }


        }
    }
}
