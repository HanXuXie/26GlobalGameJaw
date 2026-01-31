using Sirenix.OdinInspector;
using Sirenix.Serialization;
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

public enum CharaClamp
{
    [LabelText("人类")]
    Hunman,

    [LabelText("感染者")]
    Infected,
}

public class CharaBase : MonoBehaviour
{

    [LabelText("角色阵营")]
    public CharaClamp Clamp;


    [SerializeField, LabelText("血量")]
    private float m_Health;
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
    private float MaxHealth;

    [Space(10)]
    [Header("角色移动状态")]

    [LabelText("移动状态")]
    public MoveState MoveStateNow;

    [LabelText("移动速度")]
    public float MoveSpeed;

    [LabelText("到达目标点后等待时间")]
    public float ArriveWaitTime;

    private bool ifInPathing => MoveStateNow == MoveState.Pathing;

    [ShowIf("ifInPathing"), LabelText("循环路径")]
    public bool ifLoop;
    [ShowIf("ifInPathing"), LabelText("路径点列表")]
    public List<Transform> PathingPointList;

    public Vector3 MoveTarget;
    public List<Vector3> MovePath;
    public UnityAction<Vector3> OnArriveTarget;

    protected Rigidbody2D rb;
    protected Transform anchor;
    protected AnimControl_Chara animControl;
    protected VisionAttach visionAttach;
    protected StateBarMod stateBar;
    protected EntityFX entityFX;

    #region Callbacks
    // 血量改变时 [原始值，目标值]
    public UnityAction<float, float> OnHealthChange;
    #endregion


    protected virtual void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anchor = transform.Find("Anchor");
        animControl = transform.Find("Sprite").GetComponent<AnimControl_Chara>();
        visionAttach = transform.Find("Vision")?.GetComponent<VisionAttach>();
        stateBar = transform.Find("StateBar").GetComponent<StateBarMod>();
        entityFX = transform.Find("Sprite").GetComponent<EntityFX>();

        MaxHealth = Health;

        OnHealthChange += (old, now) =>
        {
            stateBar.OnHealthChange(old, now, MaxHealth);
        };
    }

    protected virtual void Start()
    {
        OnNormalUpdate += Update_Move;
    }

    protected virtual void Update()
    {
        Update_StateMachine();
        Update_Move();
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

    protected void Update_StateMachine()
    {
        // 正常状态
        if (StateNow == CharaState.Normal)
        {
            // 尝试进入警惕
            if (CanEnterAlert != null && CanEnterAlert.Invoke())
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
            if (CanEnterNormal != null && CanEnterNormal.Invoke())
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
            // 尝试进入警惕
            if (CanEnterAlert != null && CanEnterAlert.Invoke())
            {
                StateNow = CharaState.Alert;
            }
            // 尝试进入正常
            if (CanEnterNormal != null && CanEnterNormal.Invoke())
            {
                StateNow = CharaState.Normal;
            }
        }

        Debug.Log(StateNow.ToString());

        switch (StateNow)
        {
            case CharaState.Normal:
                OnNormalUpdate?.Invoke();
                break;

            case CharaState.Alert:
                OnAlertUpdate?.Invoke();
                break;

            case CharaState.Attack:
                Debug.Log("调用攻击状态");
                OnAttackUpdate?.Invoke();
                break;
        }
    }
    #endregion

    #region 更新逻辑
    private float ArriveTime;
    private void Update_Move()
    {
        // 先计算 当前真实目标 = MoveTarget + 偏差
        Vector3 targetWithOffset = MoveTarget + transform.position - anchor.position;


        // 到达目标点
        if (Vector3.Distance(anchor.position, MoveTarget) <= 0.5f)
        {
            // 停止移动
            animControl.onWalk(false);

            OnArriveTarget?.Invoke(MoveTarget);
            ArriveTime = Time.time;
            // 切换到下一个点
            if (MovePath != null && MovePath.Count > 0 && moveIndex < MovePath.Count - 1)
            {
                moveIndex++;
                MoveTarget = MovePath[moveIndex];
                ArriveTime = Time.time;

                // 循环路径处理
                if (ifLoop && moveIndex == MovePath.Count - 1)
                    moveIndex = -1;
            }
        }

        if (Time.time < ArriveTime + ArriveWaitTime) return;

        // 前往目标点
        if (MoveTarget != null && MoveTarget != Vector3.zero)
        {

            MoveTarget.z = anchor.position.z;

            if (Vector3.Distance(anchor.position, MoveTarget) >= 0.5f)
            {
                // 开始移动
                animControl.onWalk(true);
                // 根据移动方向左右翻转物体
                animControl.Flip(MoveTarget.x < transform.position.x);
                // 根据前进方向旋转视角
                visionAttach?.TurnLook(MoveTarget);
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWithOffset,
                MoveSpeed * Time.deltaTime
            );
        }

    }

    private float lastMoveTime;
    protected void Update_RandomMove(float moveRadius, float _randomMoveGap)
    {
        if (Time.time < lastMoveTime + _randomMoveGap) return;

        lastMoveTime = Time.time;

        // 当前世界位置
        Vector3 currentWorldPos = transform.position;

        // 采样点最多尝试次数
        int maxTries = 10;

        for (int i = 0; i < maxTries; i++)
        {
            // 随机点 = 当前点 + 随机方向圆形分布
            float angle = UnityEngine.Random.Range(0f, 360f);
            float radius = UnityEngine.Random.Range(1f, moveRadius);

            Vector3 randomWorldTarget = currentWorldPos + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0f
            );

            // 转成 Tilemap 格子
            Vector3Int cellFrom = SceneMod.Instance.World2Cell(currentWorldPos);
            Vector3Int cellTo = SceneMod.Instance.World2Cell(randomWorldTarget);

            // 判断是否可达
            if (SceneMod.Instance.IsConnected(cellFrom, cellTo, out List<Vector3Int> cellPath))
            {
                MovePath = SceneMod.Instance.Cell2WorldList(cellPath);
                moveIndex = 0;
                return;
            }
        }

    }
    #endregion

    #region 对外接口

    private int moveIndex = 0;

    public void MoveTo(List<Vector3> _path)
    {
        if (_path == null || _path.Count == 0) return;


        moveIndex = 0;
        MovePath = _path;
        MoveTarget = _path[0];
    }
    public void MoveTo(Vector3 _target)
    {
        Vector3Int startCell = SceneMod.Instance.Map_Barrier.WorldToCell(anchor.position);
        Vector3Int targetCell = SceneMod.Instance.Map_Barrier.WorldToCell(_target);


        if (SceneMod.Instance.IsConnected(startCell, targetCell, out List<Vector3Int> path))
        {
            var worldPosList = SceneMod.Instance.Cell2WorldList(path);
            MoveTo(worldPosList);
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        entityFX.StartWhite();
    }
    #endregion
}
