using Sirenix.OdinInspector;
using System;
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

public enum CharaState
{
    [LabelText("正常")]
    Normal,

    [LabelText("警惕")]
    Alert,

    [LabelText("攻击")]
    Attack,
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

    #region 状态机
    [Space(10)]
    [Header("角色逻辑状态")]
    public CharaState StateNow;

    public Func<bool> CanEnterNormal;
    public Func<bool> CanEnterAlert;
    public Func<bool> CanEnterAttack;

    public UnityAction OnNormalUpdate;
    public UnityAction OnAlertUpdate;
    public UnityAction OnAttackUpdate;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        // 正常状态
        if(StateNow == CharaState.Normal)
        {
            // 尝试进入警惕
            if(CanEnterAlert != null && CanEnterAlert.Invoke())
            {
                StateNow = CharaState.Alert;
            }
            // 尝试进入攻击
            if (CanEnterAttack != null && CanEnterAttack.Invoke())
            {
                StateNow = CharaState.Attack;
            }
        }
        // 警惕状态
        if (StateNow == CharaState.Alert)
        {
            // 尝试进入正常
            if (CanEnterAlert != null && CanEnterAlert.Invoke())
            {
                StateNow = CharaState.Normal;
            }
            // 尝试进入攻击
            if (CanEnterAttack != null && CanEnterAttack.Invoke())
            {
                StateNow = CharaState.Attack;
            }
        }
        // 攻击状态
        if (StateNow == CharaState.Attack)
        {
            // 尝试进入正常
            if (CanEnterAlert != null && CanEnterAlert.Invoke())
            {
                StateNow = CharaState.Normal;
            }
        }

        switch (StateNow)
        {
            case CharaState.Normal:
                OnNormalUpdate?.Invoke();
                break;

            case CharaState.Alert:
                OnAlertUpdate?.Invoke();
                break;

            case CharaState.Attack:
                OnAttackUpdate?.Invoke();
                break;
        }
    }
    #endregion
}
