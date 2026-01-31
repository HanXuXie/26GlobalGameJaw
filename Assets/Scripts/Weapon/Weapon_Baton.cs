using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Weapon_Baton : WeaponBase
{
    public override bool AttackMode(CharaBase target)
    {
        if (!isAttack) return false;

        if (target == null) return false;
        Debug.Log("攻击");

        this.transform.LookAt(target.transform, Vector3.up);
        transform.Rotate(0, -90, 0);

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > weaponRange) return false;


        List<CharaBase> charaBases = GetTargetsInCone();

        Debug.Log(charaBases.Count);

        foreach (CharaBase charaBase in charaBases)
        {
            if (charaBase.Clamp == Chara.Clamp) continue;

            charaBase.TakeDamage(weaponDamage);
            Debug.Log("击中目标");
            Debug.Log(charaBase.ToString());
        }

        weaponCooldown = weaponAttackInterval;

        return true;



    }

    private void Start()
    {
        radius = weaponRange;
    }



    [Header("扇形参数")]
    public float radius;          // 扇形半径
    public float angle = 120f;          // 扇形角度（度数）
    //public LayerMask targetLayer;      // 想要检测的层（例如：只检测敌人层）

    [Header("Gizmos 调试")]
    public Color gizmosColor = Color.yellow;

    /// <summary>
    /// 获取扇形范围内的所有目标
    /// </summary>
    /// <returns>返回 Transform 列表</returns>
    public List<CharaBase> GetTargetsInCone()
    {
        List<CharaBase> targets = new List<CharaBase>();

        // 1. 使用 Physics2D 获取半径内的所有碰撞体
        // 这一步替代了遍历全场景，性能更好
        // 注意：这里使用了 Physics2D，但不需要挂载 Collider 来接收消息，只是单纯查询
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        // 2. 遍历命中结果
        foreach (Collider2D hit in hits)
        {
            // 排除自己（虽然 OverlapCircleAll 通常不会包含自身，但加上更安全）
            if (hit.transform == transform) continue;

            // A. 计算目标相对于自己的方向
            Vector2 directionToTarget = (hit.transform.position - transform.position).normalized;

            // B. 计算自己朝向与目标方向的夹角
            // Vector2.Dot 是点积，结果 = Cos(夹角)
            // 使用 Mathf.Acos 得到弧度，再转为角度
            // 注意：如果是 3D 用 Vector3.Dot，2D 用 Vector2.Dot
            float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

            // C. 判断逻辑
            // 1. 角度必须小于设定的扇形角度的一半（因为扇形是朝前对称的）
            // 2. (可选) 再次确认距离（如果上面的 CircleAll 半径和扇形半径不一致时用）
            if (angleToTarget <= angle / 2f)
            {
                targets.Add(hit.GetComponentInParent<CharaBase>());
            }
        }

        return targets;
    }

    // 调试用：在 Scene 视图画出扇形
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        // 画个圆辅助线（可选）
        // Gizmos.DrawWireSphere(transform.position, radius);

        // 画扇形
        // 计算两条边的角度
        float leftAngle = transform.eulerAngles.z + angle / 2f;
        float rightAngle = transform.eulerAngles.z - angle / 2f;

        // 转换为向量
        Vector2 leftDir = new Vector2(Mathf.Cos(leftAngle * Mathf.Deg2Rad), Mathf.Sin(leftAngle * Mathf.Deg2Rad));
        Vector2 rightDir = new Vector2(Mathf.Cos(rightAngle * Mathf.Deg2Rad), Mathf.Sin(rightAngle * Mathf.Deg2Rad));

        // 画两条射线
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)leftDir * radius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)rightDir * radius);

        // 画圆弧（简易版，连接两条线）
        // 这里简单画个线连接两端，复杂的可以画多段弧线
    }
}
