using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon_Bite : WeaponBase
{
    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;
        if (target == null) return false;

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > weaponRange) return false;

        //调用咬合特效

        Instantiate()

        target.Health -= weaponDamage;

        return true;
    }



}



