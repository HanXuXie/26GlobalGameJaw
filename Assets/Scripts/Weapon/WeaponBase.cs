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

public class WeaponBase : MonoBehaviour
{
    public CharaBase Chara;
    public float weaponDamage;
    public float weaponAttackInterval;
    public float weaponRange;

    protected float weaponCooldown;
    protected bool isAttack = true;

    protected void Awake()
    {
        Chara = GetComponentInParent<CharaBase>();
    }


    protected virtual void Update()
    {
        if (weaponCooldown <= 0)
        {
            isAttack = true;
        }
        weaponCooldown -= Time.deltaTime;
    }

    public virtual bool AttackMode(CharaBase targets)
    {
        return false;
    }

}
