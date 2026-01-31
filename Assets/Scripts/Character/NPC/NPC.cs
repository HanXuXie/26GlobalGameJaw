using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : CharaBase
{
    public UnityAction<float, float> OnInfectionChange;
    public UnityAction<float, float> OnAlertChange;


    public float alertRadius { get; private set; }
    public float visionRadius { get; private set; }

    [LabelText("Infection")]
    public float MaxInfectionValue { get; private set; }
    public float currentInfectionValue
    {
        get
        {
            return currentInfectionValue;
        }
        set
        {
            OnInfectionChange?.Invoke(currentInfectionValue, value);
            currentInfectionValue = value;
        }
    }
    public float infectionResistance { get; private set; }

    [LabelText("Alert")]
    public float maxAlertValue { get; private set; }
    public float currentAlertValue
    {
        get
        {
            return currentAlertValue;
        }
        set
        {
            OnAlertChange?.Invoke(currentAlertValue, value);
            currentAlertValue = value;
        }
    }
    public float isAlert { get; private set; }


    public bool hasWeapon { get; private set; }

    public virtual void ChangeAlert()
    {
        
    }

    public virtual void ChangeInfection()
    {

    }

    protected virtual void Update()
    {
        ChangeAlert();
        ChangeInfection(); 
    }
}
