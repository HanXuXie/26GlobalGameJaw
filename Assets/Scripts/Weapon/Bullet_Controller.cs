using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        NPC_Infected enemy = collision.GetComponentInParent<NPC_Infected>();
        if (enemy == null) return;
        enemy.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
