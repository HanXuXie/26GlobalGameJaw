using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        CharaBase enemy = collision.GetComponent<CharaBase>();
        if (enemy == null) return;
        if (enemy.Clamp != CharaClamp.Infected) return;
        enemy.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
