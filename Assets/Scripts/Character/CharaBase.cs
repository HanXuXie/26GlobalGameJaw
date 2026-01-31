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

public class CharaBase : MonoBehaviour
{
    
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


    [Space(10)]
    [Header("角色移动状态")]

    [LabelText("移动状态")]
    public MoveState MoveStateNow;

    [LabelText("移动速度")]
    public float MoveSpeed;
    private bool ifInPathing => MoveStateNow == MoveState.Pathing;

    [ShowIf("ifInPathing"), LabelText("路径点列表")]
    public List<Transform> PathingPointList;

    #region Callbacks
    // 血量改变时 [原始值，目标值]
    public UnityAction<float, float> OnHealthChange;
    #endregion

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

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

    #region 对外接口
    public Vector3 MoveTarget;
    public List<Vector3> MovePath;
    public UnityAction<Vector3> OnArriveTarget;

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
        Vector3Int startCell = SceneMod.Instance.Map_Barrier.WorldToCell(transform.position);
        Vector3Int targetCell = SceneMod.Instance.Map_Barrier.WorldToCell(_target);


        if (SceneMod.Instance.IsConnected(startCell, targetCell, out List<Vector3Int> path))
        {
            var worldPosList = SceneMod.Instance.Cell2WorldList(path);
            MoveTo(worldPosList);
        }
    }

    private void Update_Move()
    {
        // 到达目标点
        if (Vector3.Distance(transform.position, MoveTarget) <= 0.01f)
        {
            OnArriveTarget?.Invoke(MoveTarget);
            // 切换到下一个点
            if(MovePath != null && MovePath.Count>0 && moveIndex < MovePath.Count - 1)
            {
                moveIndex++;
                MoveTarget = MovePath[moveIndex];
            }
        }

        // 前往目标点
        if (MoveTarget != null)
        {
            MoveTarget.z = transform.position.z;
            transform.position = Vector3.MoveTowards(
                transform.position,
                MoveTarget,
                MoveSpeed * Time.deltaTime
            );
        }

    }
    #endregion
}
