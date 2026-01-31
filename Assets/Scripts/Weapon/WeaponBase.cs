using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum WeaponType
//{
//    Bite,
//    Baton,
//    Pistol
//}

public class WeaponBase : MonoBehaviour
{
    public float weaponDamage;
    public float weaponAttackInterval;
    public float weaponRange;

    protected float weaponCooldown;
    protected bool isAttack = true;

    protected virtual void Update()
    {
        if (weaponCooldown <= 0)
        {
            isAttack = true;
        }
        weaponCooldown -= Time.deltaTime;
    }

    protected virtual bool AttackMode(List<CharaBase> targets)
    {
        return false;
    }

}
