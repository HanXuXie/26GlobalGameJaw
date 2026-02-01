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
                hasAlert = true;
            }
            if (collider.GetComponentInParent<NPC_Civilian>())
            {
                Debug.Log("检测到良民");
                CurrentAlertValue += 100;
                hasAlert = true;
            }
        }

        if (!hasAlert)
        {
            CurrentAlertValue -= Time.deltaTime * alertChangeSpeed;
        }

    }
}
