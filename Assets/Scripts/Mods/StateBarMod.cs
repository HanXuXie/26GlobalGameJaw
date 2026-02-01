using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StateBarMod : MonoBehaviour
{
    public static float FadeDeltaTime = 3f;

    public Transform Infection;
    public Transform Health;

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
        if(Infection.gameObject.activeSelf == true)
        {
            if(Time.time > LastChangeTime_Infection + FadeDeltaTime)
            {
                Infection.gameObject.SetActive(false);
            }
        }

        if (Health.gameObject.activeSelf == true)
        {
            if(Time.time > LastChangeTime_Health + FadeDeltaTime)
            {
                Health.gameObject.SetActive(false);
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

    public void OnInfectionChange(float _old, float _now, float _total)
    {
        LastChangeTime_Infection = Time.time;

        Infection.gameObject.SetActive(true);

        // 更新条长度
        UpdateBar(Infection_old.transform, _old, _total, OrginScale_Infection);
        UpdateBar(Infection_now.transform, _now, _total, OrginScale_Infection);
    }

    public void OnHealthChange(float _old, float _now, float _total)
    {
        LastChangeTime_Health = Time.time;

        Health.gameObject.SetActive(true);

        // 更新条长度
        UpdateBar(Health_old.transform, _old, _total, OrginScale_Health);
        UpdateBar(Health_now.transform, _now, _total, OrginScale_Health);

    }

    #region 工具函数

    private void UpdateBar(Transform _bar, float _valueX, float _total, float _originalScaleX)
    {
        // 缩放
        Vector3 newScale = _bar.localScale;
        newScale.x = (_valueX / _total) * _originalScaleX;
        _bar.localScale = newScale;

        float deltaScale = _originalScaleX - newScale.x;

        Vector3 pos = _bar.localPosition;
        pos.x = -deltaScale * 0.5f; // 向左移动
        _bar.localPosition = pos;

    }
    #endregion
}
