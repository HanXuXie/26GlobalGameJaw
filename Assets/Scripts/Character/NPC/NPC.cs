using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : CharaBase
{
    public UnityAction<float, float> OnInfectionChange;
    public UnityAction<float, float> OnAlertChange;

    public Collider2D Vision;

    public float CurrentVisionTime;
    public float MaxVisionTime = 1f;

    [field: SerializeField] public float visionRadius { get; private set; }

    public LayerMask detectionMask;

    List<CharaBase> CharaBases = new List<CharaBase>();

    #region 追踪视野内最近的敌人

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);

        Debug.Log(detectionMask.value);

        if (collision.gameObject.layer == 9)
        {
            return;
        }

        CharaBase target = collision.GetComponentInParent<CharaBase>();

        if (target != null && !CharaBases.Contains(target))
        {
            CharaBases.Add(target);
        }

        AttackTargetUpdate();

    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        CharaBase target = collision.GetComponentInParent<CharaBase>();
        if (target != null && CharaBases.Contains(target))
        {
            CharaBases.Remove(target);
        }

        AttackTargetUpdate();
    }
    public void AttackTargetUpdate()
    {
        CharaBase newTarget = TargetAcquisition.VisionRangeNearestEnemy(this, CharaBases);

        if (newTarget != null)
        {
            CurrentVisionTime = MaxVisionTime;
            attackTarget = newTarget;
        }

        if (CurrentVisionTime < 0)
        {
            attackTarget = null;
        }

        if (attackTarget != null)
        {
            attackAimTarget = attackTarget.transform.Find("Anchor");
        }

    }

    #endregion

    #region 感染值相关

    [field: SerializeField] public float maxInfectionValue { get; private set; } // 感染值相关

    public float CurrentInfectionValue
    {
        get
        {
            return m_currentInfectionValue;
        }
        set
        {
            OnInfectionChange?.Invoke(m_currentInfectionValue, value);
            m_currentInfectionValue = value;
        }

    }
    [SerializeField] private float m_currentInfectionValue;

    [field: SerializeField]
    public float InfectionResistance { get; private set; }

    [SerializeField] private bool isInfection;

    #endregion

    #region 警惕值机制

    [field: SerializeField]
    public float maxAlertValue { get; private set; }   //  警惕值相关

    [field: SerializeField] public float alertChangeSpeed { get; protected set; }

    public float CurrentAlertValue
    {
        get
        {
            return m_currentAlertValue;
        }
        set
        {
            OnAlertChange?.Invoke(m_currentAlertValue, value);
            m_currentAlertValue = value;
        }
    }
    [SerializeField] private float m_currentAlertValue;

    [field: SerializeField] public float alertRadius { get; private set; }

    [field: SerializeField] public bool isAlert { get; private set; }

    public float AlertTime;

    public float MaxAlertTime = 5;

    #endregion

    #region 武器攻击机制相关

    [field: SerializeField] public bool isAttack { get; private set; }

    [field: SerializeField] public CharaBase attackTarget = null;

    public Transform attackAimTarget;

    [field: SerializeField] public bool hasWeapon { get; private set; }

    public WeaponBase Weapon;

    #endregion

    #region 移动相关

    public float MoveRadius;
    public float MoveCooldown;

    #endregion

    #region 警惕值和感染值检测和改变相关

    public virtual void ChangeAlert()
    {

        if (CurrentAlertValue > maxAlertValue)
        {
            CurrentAlertValue = maxAlertValue;
        }

        if (CurrentAlertValue < 0)
        {
            CurrentAlertValue = 0;
        }
    }

    public virtual void ChangeInfection(float infectionSpeed)
    {

    }



    private void AlertDetection()
    {

        if (CurrentAlertValue >= maxAlertValue && !isAlert)
        {
            isAlert = true;
            AlertTime = MaxAlertTime;
        }
        else if (CurrentAlertValue < maxAlertValue || AlertTime <= 0)
        {
            isAlert = false;
        }

    }

    private void InfectionDetection()
    {

        if (CurrentInfectionValue >= maxInfectionValue)
        {
            isInfection = true;
        }
        else
        {
            isInfection = false;
        }
    }


    protected virtual void AttackDetection()
    {
        if (attackTarget != null)
        {
            isAttack = true;
        }
    }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Vision")
                Vision = collider;
        }

        OnInfectionChange += (old, now) =>
        {
            stateBar.OnInfectionChange(old, now, maxInfectionValue);
            if (now == maxAlertValue && !isInfection)
            {
                isInfection = true;
                HasInfected();
            }
        };
    }

    protected override void Start()
    {
        base.Start();

        Weapon = GetComponentInChildren<WeaponBase>();
        if (Weapon != null) hasWeapon = true;


        CanEnterNormal += () =>
        {
            if (!isAlert && !isAttack)
            {
                Debug.Log("进入正常状态");
                return true;
            }
            else
                return false;
        };

        CanEnterAlert += () =>
        {
            Debug.Log("尝试进入警戒状态");
            if (isAlert && !isAttack)
            {
                Debug.Log("进入警戒状态");
                return true;
            }
            else
                return false;
        };

        CanEnterAttack += () =>
        {
            if (isAlert && isAttack)
                return true;
            else
                return false;

        };

        OnNormalUpdate += () =>
        {
            Debug.Log("正常状态");
            switch (MoveStateNow)
            {
                case MoveState.Static:
                    break;

                case MoveState.Pathing:
                    if (PathingPointList != null && PathingPointList.Count > 0 && (MovePath == null || MovePath.Count == 0))
                    {
                        List<Vector3> WordPath = new List<Vector3>();
                        PathingPointList.ForEach(point => WordPath.Add(point.position));
                        MoveTo(WordPath);
                    }
                    break;

                case MoveState.Random:
                    Update_RandomMove(10, 10);
                    break;
            }
        };

        OnAlertUpdate += AlertUpdate;

        OnAttackUpdate += AttackUpdate;
    }

    protected virtual void AlertUpdate()
    {

    }

    protected virtual void AttackUpdate()
    {

    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, alertRadius);
        // 3. 设置描边颜色（通常比填充色深一点）
        Gizmos.color = Color.white;

        // 4. 绘制线框圆（描边）
        // 这里使用DrawWireSphere绘制球体的线框，对于2D来说就是圆
        Gizmos.DrawWireSphere(transform.Find("Anchor").position, alertRadius);
    }




    protected override void Update()
    {
        base.Update();
        AlertTime -= Time.deltaTime;
        CurrentVisionTime -= Time.deltaTime;

        Vision.transform.localScale = new Vector3(visionRadius, visionRadius, visionRadius);
        ChangeAlert();
        AlertDetection();
        InfectionDetection();
        AttackDetection();
    }



}
