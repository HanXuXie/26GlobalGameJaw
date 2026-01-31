using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_pistol : WeaponBase
{
    public GameObject BulletPrefab;
    public Animator Animator;


    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;

        if (target == null) return false;
        Debug.Log("攻击");

        Vector3 aimTran = target.transform.position + new Vector3(0, 2f, 0);

        this.transform.LookAt(aimTran, Vector3.up);
        transform.Rotate(0, -90, 0);

        Animator.SetBool("Shot", true);

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);


        bullet.transform.Rotate(0, 0, Random.Range(-7, 7));

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 20;

        StartCoroutine(DestroyBullet(bullet));

        weaponCooldown = weaponAttackInterval;

        return true;
    }

    protected override void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void ToIdle()
    {
        Animator.SetBool("Shot", false);
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);

        Destroy(bullet);
    }
}
