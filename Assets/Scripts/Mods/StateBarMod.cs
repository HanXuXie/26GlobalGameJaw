using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBarMod : MonoBehaviour
{
    public static float FadeDeltaTime = 5f;

    public SpriteRenderer Infection_old;
    public SpriteRenderer Infection_now;
    public SpriteRenderer Health_old;
    public SpriteRenderer Health_now;

    private float OrginScale_Infection;
    private float OrginScale_Health;

    private float LastChangeTime_Infection;
    private float LastChangeTime_Health;

    private void Awake()
    {
        OrginScale_Infection = Infection_now.transform.localScale.x;
        OrginScale_Health = Health_now.transform.localScale.x;
    }

    private void Update()
    {
        if(Infection_now.gameObject.activeSelf == true)
        {
            if(Time.time > LastChangeTime_Infection + FadeDeltaTime)
            {
                Infection_now.gameObject.SetActive(false);
                Infection_old.gameObject.SetActive(false);
            }
        }

        if (Health_now.gameObject.activeSelf == true)
        {
            if(Time.time > LastChangeTime_Health + FadeDeltaTime)
            {
                Health_now.gameObject.SetActive(false);
                Health_old.gameObject.SetActive(false);
            }
        }
    }

    #region Test
    [Button("感染条测试")]
    public void Test_Infection()
    {
        OnInfectionChange(100, 80, 100);
    }

    [Button("血条测试")]
    public void Test_Health()
    {
        OnHealthChange(100, 80, 100);
    }
    #endregion

    public void OnInfectionChange(float _old, float _new, float _total)
    {
        LastChangeTime_Infection = Time.time;

        Infection_now.gameObject.SetActive(true);
        Infection_old.gameObject.SetActive(true);

        // 更新条长度
        SetLoaclScale_X(Infection_now.transform, OrginScale_Infection * (_old / _total));
        SetLoaclScale_X(Infection_old.transform, OrginScale_Infection * (_old / _total));

    }

    public void OnHealthChange(float _old, float _now, float _total)
    {
        LastChangeTime_Health = Time.time;

        Health_now.gameObject.SetActive(true);
        Health_old.gameObject.SetActive(true);

        // 更新条长度
        SetLoaclScale_X(Health_old.transform, OrginScale_Health * (_old / _total));

        SetLoaclScale_X(Health_now.transform, OrginScale_Health * (_now / _total));

    }

    #region 工具函数
    public void SetLoaclScale_X(Transform _base, float x)
    {
        _base.localScale = new Vector3(x, _base.localScale.y, _base.localScale.z);
    }
    #endregion
}
