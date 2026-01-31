using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_pistol : WeaponBase
{
    public GameObject BulletPrefab;



    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;

        if (target == null) return false;
        Debug.Log("攻击");

        this.transform.LookAt(target.transform, Vector3.up);
        transform.Rotate(0, -90, 0);

        GameObject bullet = Instantiate(BulletPrefab, target.transform.position, target.transform.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,1);

        StartCoroutine(DestroyBullet(bullet));

        weaponCooldown = weaponAttackInterval;

        return true;
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);

        Destroy(bullet);
    }
}
