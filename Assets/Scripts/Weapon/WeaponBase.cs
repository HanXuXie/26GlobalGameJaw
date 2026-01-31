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
    [LabelText("Weapon Attribution")]
    public float weaponDamage { get; private set; }
    public float weaponAttackInterval { get; private set; }
    public float weaponRange { get; private set; }

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

    protected virtual bool AttackMode(CharaBase[] targets)
    {
        return false;
    }

}
