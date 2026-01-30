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
    [SerializeField] protected float weaponDamage;
    [SerializeField] protected float weaponAttackInterval;
    [SerializeField] protected float weaponRange;

    protected virtual void AttackMode(CharaBase[] targets)
    {

    }

}
