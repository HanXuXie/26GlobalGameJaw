using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeAttach : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        NPC npc = null;
        if ((npc = other.GetComponent<NPC>() )!= null)
        {
            npc.ChangeInfection(1);
        }
    }
}
