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
    public bool isAttack = true;

    protected virtual void Awake()
    {
        Chara = GetComponentInParent<CharaBase>();
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
