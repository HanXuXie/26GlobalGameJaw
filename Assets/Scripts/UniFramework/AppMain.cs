using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

    [LabelText("怀疑值变化_每秒上升")]
    public float WarningChange_PreSecond = 1;
    [LabelText("怀疑值变化_发现变身玩家")]
    public float WarningChange_FindPlayer = 25;
    [LabelText("怀疑值变化_发现感染者")]
    public float WarningChange_FindInfector = 100;
    [LabelText("怀疑值变化_转变感染者")]
    public float WarningChange_ChangeHunman = 10;

    [LabelText("是否爆发")]
    public bool IfBoom;

    [LabelText("小人列表")]
    public List<CharaBase> CharaList = new();

    [LabelText("待变化小人列表")]
    public List<CharaBase> InfectedCharaList = new();

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
    public Image Image_HunmanStrip;
    public Image Image_InfectionStrip;

    public GameObject BoomEffectPerfab;
    public GameObject InfectonPerfab;

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
        ChangeWarningNum(WarningChange_PreSecond * Time.deltaTime);
    }
    #region 回调函数


    #endregion
    #region 对外接口
    // 爆发感染
    public void Boom()
    {
        // 关闭视野
        foreach(var chara in CharaList)
        {
            if(chara != null)
                chara.transform.Find("Vision")?.gameObject.SetActive(false);
        }

        // 转变感染者
        var InfectedCharaListChache = new List<CharaBase>(InfectedCharaList);
        foreach (var chara in InfectedCharaListChache)
        {
            if(chara != null)
            {
                var boomObj = Instantiate(BoomEffectPerfab, transform, transform);
                boomObj.transform.position = chara.transform.position;
                boomObj.gameObject.SetActive(true);

                var infectonObj = Instantiate(InfectonPerfab);
                infectonObj.transform.position = chara.transform.position;

                Destroy(chara.gameObject);
            }
        }
    }

    // 转变一个人类
    public void ChangeOneHunman(CharaBase _chara)
    {
        HumanNum = Mathf.Max(HumanNum - 1, 0);
        InfectorNum = Mathf.Max(InfectorNum + 1, 0);
        ChangeWarningNum(WarningChange_ChangeHunman);
        RefreshTopBar();
        InfectedCharaList.Add(_chara);
    }

    // 发现变身后的玩家
    public void FindPlayer_Henshin()
    {
        ChangeWarningNum(WarningChange_FindPlayer);
    }

    // 发现感染者
    public void FindInfector()
    {
        ChangeWarningNum(WarningChange_FindInfector);
    }

    // 小人Start注册
    public void CharaRegist(CharaBase _charaBase)
    {
        CharaList.Add(_charaBase);
        if(_charaBase.Clamp == CharaClamp.Hunman)
        {
            HumanNum++;
        }
        else
        {
            InfectorNum++;
        }
        RefreshTopBar();
    }

    // 改变警惕值
    public void ChangeWarningNum(float _value)
    {
        float oldValue = WarningNum;
        WarningNum = Mathf.Clamp(WarningNum + _value, 0, MaxWarningNum);
        if (oldValue <= 25 && WarningNum > 25)
        {
            AudioManager.instance.PlayEffect(MusicEffectType.一级警报, gameObject);
        }
        if (oldValue <= 50 && WarningNum > 50)
        {
            AudioManager.instance.PlayEffect(MusicEffectType.二级警报, gameObject);
            AudioManager.instance.PlayMusic(MusicType.State2);
        }
        if (oldValue <= 75 && WarningNum > 75)
        {
            AudioManager.instance.PlayEffect(MusicEffectType.三级警报, gameObject);
            AudioManager.instance.PlayMusic(MusicType.State3);
        }
    }

    public void RefreshTopBar()
    {
        var text_InfectorNum = transform.Find("TopBar/Text_InfectorNum").GetComponent<TextMeshProUGUI>();
        var text_HumanNum = transform.Find("TopBar/Text_HumanNum").GetComponent<TextMeshProUGUI>();

        Image_HunmanStrip.fillAmount = (float)HumanNum / (float)(InfectorNum + HumanNum);
        Image_InfectionStrip.fillAmount = (float)InfectorNum / (float)(InfectorNum + HumanNum);

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
