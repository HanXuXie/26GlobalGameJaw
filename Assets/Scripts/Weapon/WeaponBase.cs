using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum WeaponType
//{
//    Bite,
//    Baton,
//    Pistol
//}

public abstract class WeaponBase : MonoBehaviour
{
    public CharaBase Chara;
    public float weaponDamage;
    public float weaponAttackInterval;
    public float weaponRange;

    public float weaponCooldown;
    public bool isAttack = false;

    protected virtual void Awake()
    {
        
    }


    protected virtual void Update()
    {
        if (weaponCooldown <= 0)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
        weaponCooldown -= Time.deltaTime;
    }

    public abstract bool AttackMode(CharaBase target);


}
