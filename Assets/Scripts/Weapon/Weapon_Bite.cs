using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon_Bite : WeaponBase
{
    private GameObject BitePrefab;




    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;
        if (target == null) return false;

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > weaponRange) return false;

        weaponCooldown = weaponAttackInterval;

        //调用咬合特效

        GameObject bite = Instantiate(BitePrefab, target.transform);

        StartCoroutine(DestroyBite(bite));

        target.TakeDamage(weaponDamage);

        weaponCooldown = weaponAttackInterval;

        return true;
    }

    IEnumerator DestroyBite(GameObject bite)
    {
        yield return new WaitForSeconds(0.2f);

        Destroy(bite);
    }

}



