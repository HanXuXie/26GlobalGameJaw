using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int WarningNum;

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

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // 播放音乐 & 注册回调
        AudioManager.instance.PlayMusic(MusicType.State1_begin)
            .OnComplete(OnMusicComplete)
            .Play();
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
}
