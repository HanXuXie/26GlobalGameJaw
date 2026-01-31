using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : CharaBase
{
    public Collider2D Vision;

    public LayerMask detectionMask;

    List<CharaBase> CharaBases = new List<CharaBase>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);

        Debug.Log(detectionMask.value);

        if (collision.gameObject.layer == 9)
        {
            Debug.Log("检测到视野层");
            return;
        }

        Debug.Log(collision.ToString());

        CharaBase target = collision.GetComponentInParent<CharaBase>();

        if (target != null && !CharaBases.Contains(target))
        {
            CharaBases.Add(target);
        }

        attackTarget = TargetAcquisition.VisionRangeNearestEnemy(this, CharaBases);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        CharaBase target = collision.GetComponentInParent<CharaBase>();
        if (target != null && CharaBases.Contains(target))
        {
            CharaBases.Remove(target);
        }
        attackTarget = TargetAcquisition.VisionRangeNearestEnemy(this, CharaBases);
    }












    public UnityAction<float, float> OnInfectionChange;
    public UnityAction<float, float> OnAlertChange;

    [field: SerializeField] public float alertRadius { get; private set; }
    [field: SerializeField] public float visionRadius { get; private set; }

    [field: SerializeField] public float maxInfectionValue { get; private set; } // 感染值相关

    [ShowInInspector]
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


    [field: SerializeField]
    public float maxAlertValue { get; private set; }   //  警惕值相关

    [field: SerializeField] public float alertChangeSpeed { get; protected set; }

    [ShowInInspector]
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

    [field: SerializeField] public bool isAlert { get; private set; }



    [field: SerializeField] public bool isAttack { get; private set; }
    [field: SerializeField] public CharaBase attackTarget = null;



    [field: SerializeField] public bool hasWeapon { get; private set; }

    #region 警惕值和感染值检测和改变相关

    public virtual void ChangeAlert()
    {

    }

    public virtual void ChangeInfection(float infectionSpeed)
    {

    }



    private void AlertDetection()
    {
        if (CurrentAlertValue >= maxAlertValue)
        {
            isAlert = true;
        }
        else
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
        if(attackTarget != null)
        {
            isAttack = true;
        }
    }
    #endregion

    protected void Awake()
    {
        Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.name = "Vision";
            Vision = collider;
        }
    }

    protected override void Start()
    {



        CanEnterNormal += () =>
        {
            if (!isAlert && !isAttack)
                return true;
            else
                return false;
        };

        CanEnterAlert += () =>
        {
            if (isAlert)
                return true;
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
            switch (MoveStateNow)
            {
                case MoveState.Static:
                    break;

                case MoveState.Pathing:
                    if(PathingPointList != null && PathingPointList.Count > 0)
                    {
                        List<Vector3> WordPath = new List<Vector3>();
                        PathingPointList.ForEach(point=>WordPath.Add(point.position));
                        MoveTo(WordPath);
                    }
                    break;

                case MoveState.Random:
                    Update_RandomMove(10, 10);
                    break;
            }
        };

        OnAlertUpdate += () =>
        {

        };

        OnAttackUpdate += () =>
        {

        };
    }

    protected override void Update()
    {
        base.Update();
        Vision.transform.localScale = new Vector3(visionRadius, visionRadius, visionRadius);
        ChangeAlert();
        AlertDetection();
        InfectionDetection();



    }



}
