using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeAttach : MonoBehaviour
{
    public float damage = 1;
    private void OnTriggerStay2D(Collider2D collision)
    {
        NPC npc = null;
        if ((npc = collision.GetComponentInParent<NPC>()) != null)
        {
            npc.ChangeInfection(damage);
        }
    }
}
