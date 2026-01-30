using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveState
{
    [LabelText("静止")]
    Static,

    [LabelText("路径")]
    Pathing,

    [LabelText("随机")]
    Random
}

public class CharaBase : MonoBehaviour
{
    // 血量改变时 [原始值，目标值]
    public UnityAction<float, float> OnHealthChange;

    [ShowInInspector, LabelText("血量")]
    public float Health
    {
        get
        {
            return m_Health;
        }
        set
        {
            OnHealthChange?.Invoke(m_Health, value);
            m_Health = value;
        }
    }
    private float m_Health;

    [ShowInInspector, LabelText("移动速度")]
    public float MoveSpeed;

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }
}
