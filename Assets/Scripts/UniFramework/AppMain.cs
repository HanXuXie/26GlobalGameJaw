using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;

public enum WarningState
{
    STATE1,
    STATE2,
    STATE3,
    STATE4,
}

public class AppMain : MonoBehaviour
{
    public static AppMain instance;

    [LabelText("人类数量")]
    public int HumanNum;

    [LabelText("感染者数量")]
    public int InfectorNum;

    [LabelText("怀疑值")]
    public float WarningNum;

    [LabelText("怀疑值上限")]
    public float MaxWarningNum = 100;

    [LabelText("是否爆发")]
    public bool IfBoom;

    public WarningState WarningState
    {
        get
        {
            if (WarningNum <= 25)
                return WarningState.STATE1;
            if (WarningNum <= 50)
                return WarningState.STATE2;
            if (WarningNum <= 75)
                return WarningState.STATE3;
            else
                return WarningState.STATE4;
        }
    }
    
    public Image Image_WarningStrip;
    public Image Image_WarningArrow;


    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            enabled = false;
            return;
        }
        RefreshTopBar();
        RefreshRightBar();
    }

    private void Start()
    {
        // 播放音乐 & 注册回调
        AudioManager.instance.PlayState1Music();
    }
    private void Update()
    {
        RefreshRightBar();
    }
    #region 回调函数

    public void OnMusicComplete()
    {
        // 爆发后
        if(IfBoom)
        {
            AudioManager.instance.PlayMusic(MusicType.Boom);
        }
        else if(WarningState == WarningState.STATE1)
        {
            AudioManager.instance.PlayMusic(MusicType.State1_middle);
        }
        else if(WarningState == WarningState.STATE2)
        {
            AudioManager.instance.PlayMusic(MusicType.State2);
        }
        else if (WarningState == WarningState.STATE3)
        {
            AudioManager.instance.PlayMusic(MusicType.State3);
        }
        else if (WarningState == WarningState.STATE4)
        {
            AudioManager.instance.PlayMusic(MusicType.State4);
        }
    }

    #endregion
    #region 对外接口
    public void ChangeWarningNum(float _value)
    {
        WarningNum = Mathf.Clamp(WarningNum + _value, 0, MaxWarningNum);
    }

    public void RefreshTopBar()
    {
        var text_InfectorNum = transform.Find("TopBar/Text_InfectorNum").GetComponent<TextMeshProUGUI>();
        var text_HumanNum = transform.Find("TopBar/Text_HumanNum").GetComponent<TextMeshProUGUI>();

        text_InfectorNum.text = $"感染数:{InfectorNum.ToString()}";
        text_HumanNum.text = $"人类数:{HumanNum.ToString()}";
    }

    public void RefreshRightBar()
    {
        Image_WarningStrip.fillAmount = WarningNum / MaxWarningNum;
        float angleZ = -310 - (560 - 310) * (WarningNum / MaxWarningNum);
        Image_WarningArrow.transform.eulerAngles = new Vector3 (0, 0, angleZ);
    }
    #endregion
}
