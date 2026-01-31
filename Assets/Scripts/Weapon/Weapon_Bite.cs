using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon_Bite : WeaponBase
{
    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;
        if (target == null) return false;
        target.Health -= weaponDamage;
        //调用咬合特效

        return true;
    }



}



