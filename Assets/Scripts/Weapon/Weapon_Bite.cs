using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon_Bite : WeaponBase
{
    public GameObject BitePrefab;




    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;
        if (target == null) return false;

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > weaponRange) return false;

        Vector3 aimTran = target.transform.position + new Vector3(0, 2f, 0);

        weaponCooldown = weaponAttackInterval;

        GameObject Object = Instantiate(BitePrefab, aimTran, target.transform.rotation, target.transform);

        AudioManager.instance.PlayEffect(MusicEffectType.噬咬, Object);

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



