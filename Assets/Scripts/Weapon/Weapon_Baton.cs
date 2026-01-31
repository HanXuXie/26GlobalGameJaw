using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Baton : WeaponBase
{
    protected override bool AttackMode(List<CharaBase> targets)
    {
        if (!isAttack) return false;

        CharaBase target = TargetAcquisition.WeaponRangeNearestEnemy(this, targets);

        if(target == null) return false;

        return true;
       

    }
}
