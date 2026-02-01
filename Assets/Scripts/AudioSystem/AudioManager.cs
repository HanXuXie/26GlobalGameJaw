using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> 音乐类型 </summary>
public enum MusicType
{
    None,

    State1_begin,
    State1_middle,

    State2,
    State3,
    State4,
    Boom,
}

/// <summary> 音效类型 </summary>
public enum MusicEffectType
{
    None,

    // UI交互音效
    ui点击_沉闷,
    ui点击_电子,
    ui点击_清脆,

    // 玩家交互音效
    持续普攻,
    大招,
    戴面具,
    技能_毒瓶,

    // 基地警报音效
    一级警报,
    二级警报,
    三级警报,

    // 武器交互音效
    棍击,
    枪声,
    噬咬,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [LabelText("音乐大小"), Range(0, 1)]
    public float MusicAmount = 0.5f;
    [LabelText("音效大小"), Range(0, 1)]
    public float MusicEffectAmount = 0.5f;
    [LabelText("当前播放音乐"), ShowInInspector, ReadOnly]
    public MusicType OnPlayMusic { get; private set; }

    [Space(10), Header("对象池")]
    [ShowInInspector, ReadOnly]
    private Queue<GameObject> ObjPool_EffectSource;

    // 音乐播放器[单个]
    private AudioSource MusicSource;

    // 音效播放器[预制体]
    private GameObject EffectSourcePerfab;

    private Transform objPool;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            enabled = false;
            return;
        }

        MusicSource = transform.Find("MusicSource").GetComponent<AudioSource>();
        EffectSourcePerfab = transform.Find("EffectSource").gameObject;

        objPool = transform.Find("ObjPool");

        ObjPool_EffectSource = new();

        // 同步音量大小
        //RefreshMusicAmount(AppMain.instance.MusicAmount);
        //RefreshEffectAmount(AppMain.instance.MusicEffectAmount);
    }

    // 刷新音乐音量
    public void RefreshMusicAmount(float _amount)
    {
        if(MusicAmount == _amount) return;

        MusicSource.volume = MusicAmount;
        MusicAmount = _amount;
    }

    // 刷新音效音量
    public void RefreshEffectAmount(float _amount)
    {
        if(MusicEffectAmount == _amount) return;


        if(ObjPool_EffectSource != null && ObjPool_EffectSource.Count > 0)
        {
            foreach(var source in ObjPool_EffectSource)
                if(source != null)
                {
                    source.GetComponent<AudioSource>().volume = MusicEffectAmount;
                }
        }
        MusicEffectAmount = _amount;
    }


    public AudioSourceAttach PlayEffect(MusicEffectType _effectType, GameObject _linkObj)
    {
        AudioClip clip = Resources.Load<AudioClip>($"AudioClips/Effect/{_effectType}");

        if(clip != null)
        {
            ObjPool_EffectSource = new Queue<GameObject>(ObjPool_EffectSource.Where(obj => obj != null));

            GameObject obj = null;
            // 加载音效播放器
            if(ObjPool_EffectSource.Count > 0)
                obj = ObjPool_EffectSource.Dequeue();
            else
            {
                obj = Instantiate(EffectSourcePerfab, _linkObj.transform);
                obj.transform.localPosition = Vector3.zero;
            }

            obj.gameObject.SetActive(true);
            var audioSource = obj.GetComponent<AudioSource>();
            audioSource.volume = MusicEffectAmount;
            audioSource.clip = clip;

            var audioAttack = obj.GetComponent<AudioSourceAttach>();
            audioAttack.OnComplete(() => {
                if(audioAttack != null)
                {
                    audioAttack.DestoryData();
                    audioAttack.transform.SetParent(objPool);
                    audioAttack.gameObject.SetActive(false);
                    ObjPool_EffectSource.Enqueue(obj);
                }
            });
            audioAttack.Play();
            return audioAttack;
        }
        return null;
    }

    public void PlayMusic(MusicType _musicType, bool _playImmediately = false)
    {
        AudioClip clip = Resources.Load<AudioClip>($"AudioClips/Music/{_musicType}");

        if (clip != null)
        {
            MusicSource.clip = clip;
            MusicSource.gameObject.SetActive(true);
            if (_musicType != OnPlayMusic || _playImmediately)
            {
                MusicSource.Play();
            }
            OnPlayMusic = _musicType;
        }
    }

    public void PlayState1Music()
    {
        AudioClip clip = Resources.Load<AudioClip>($"AudioClips/Music/{MusicType.State1_begin}");
        MusicSource.gameObject.SetActive(true);
        OnPlayMusic = MusicType.State1_begin;
        MusicSource.clip = clip;
    }

    IEnumerable SwitchToMiddle(float _clipTime)
    {
        yield return new WaitForSeconds(_clipTime);
        AudioClip clip = Resources.Load<AudioClip>($"AudioClips/Music/{MusicType.State1_middle}");
        MusicSource.clip = clip;
        MusicSource.Play();
        OnPlayMusic = MusicType.State1_middle;
    }
}
