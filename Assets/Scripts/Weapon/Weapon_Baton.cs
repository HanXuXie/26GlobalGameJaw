using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Baton : WeaponBase
{
    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;

        if (target == null) return false;

        this.transform.LookAt(target.transform);

        return true;



    }
}
