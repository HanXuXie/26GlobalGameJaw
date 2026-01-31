using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class NPC_Guard : NPC
{
    public override void ChangeAlert()
    {
        base.ChangeAlert();
        Collider[] Colliders = Physics.OverlapSphere(transform.position, alertRadius);
        foreach(Collider collider in Colliders)
        {
            if (collider.GetComponent <NPC_Civilian>())
            {

            }
            //获取行为

        }

    }

    public override void ChangeInfection()
    {
        base.ChangeInfection();
    }

    protected override void Update()
    {
        base.Update();
    }
}
