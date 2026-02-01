using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UGUIAnimator : MonoBehaviour
{
    [LabelText("播放状态"), ReadOnly, ShowInInspector]
    public bool PlayerState { get { return m_playerState; } }
    [LabelText("当前帧索引"), ReadOnly, ShowInInspector]
    public int FrameIndex { get { return m_frameIndex; } }
    [LabelText("动画序列帧")]
    public Sprite[] FrameList;

    [Space(10)]
    [Header("播放预设")]
    [LabelText("唤醒时播放")]
    public bool IsPlayOnAwake = true;
    [LabelText("是否为循环")]
    public bool IsLoop = true;
    [LabelText("帧间间隔")]
    public float FrameGap = 0.2f;
    [LabelText("单次播放结束事件")]
    public UnityEvent CompleteEvent;

    private Image m_Image;
    // 播放状态
    private bool m_playerState = false;
    // 当前帧索引
    private int m_frameIndex = 0;
    // 循环协程
    private Coroutine m_loopCoroutine;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Init();
        if (IsPlayOnAwake)
            Play();
    }

    private void OnDisable()
    {
        Stop();
    }

    private void OnDestroy()
    {
        if (m_loopCoroutine != null)
            StopCoroutine(m_loopCoroutine);
        m_loopCoroutine = null;
    }

    public void Init()
    {
        m_playerState = false;
        m_frameIndex = 0;
        if (m_loopCoroutine != null)
            StopCoroutine(m_loopCoroutine);

    }

    [Button("播放", ButtonHeight = 30), GUIColor("green"), HideIf("PlayerState")]
    public void Play()
    {
        if (m_playerState) return;

        m_playerState = true;
        m_loopCoroutine = StartCoroutine(AnimUpdate());
    }

    [Button("暂停", ButtonHeight = 30), GUIColor("red"), ShowIf("PlayerState")]
    public void Stop()
    {
        if (!m_playerState) return;

        m_playerState = false;
        if (m_loopCoroutine != null)
            StopCoroutine(m_loopCoroutine);
        m_loopCoroutine = null;
    }

    IEnumerator AnimUpdate()
    {
        var frameGap = FrameGap;
        var coroutoneGap = new WaitForSeconds(frameGap);
        while (m_playerState)
        {
            // 同步帧间隔
            if (FrameGap != frameGap)
            {
                frameGap = FrameGap;
                coroutoneGap = new WaitForSeconds(frameGap);
            }

            // 更新帧
            if (m_playerState && FrameList != null && FrameList.Length > 0)
            {
                if(m_frameIndex == FrameList.Length - 1)
                {
                    CompleteEvent?.Invoke();
                    if (!IsLoop)
                    {
                        gameObject.SetActive(false);
                        break;
                    }
                }
                m_frameIndex = m_frameIndex % FrameList.Length;
                m_Image.sprite = FrameList[m_frameIndex];
                m_frameIndex++;
                yield return coroutoneGap;
            }
            yield return null;
        }
    }
}
