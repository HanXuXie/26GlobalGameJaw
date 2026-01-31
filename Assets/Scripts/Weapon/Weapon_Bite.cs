using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon_Bite : WeaponBase
{
    protected override bool AttackMode(CharaBase[] targets)
    {
        if (!isAttack) return false;
        CharaBase target = TargetAcquisition.NearestEnemy(this, targets);
        if (target == null) return false;
        target.Health -= weaponDamage;
        //调用咬合特效

        return true;
    }



}



