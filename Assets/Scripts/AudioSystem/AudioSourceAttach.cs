using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSourceAttach : MonoBehaviour
{
    private AudioSource audioSource;

    public UnityAction OnCompleteEvent;

    public AttackInfo OnPlay { get; private set; }
    public float volume 
    { 
        get => audioSource.volume;
        set { audioSource.volume = value; }
    }
    public AudioClip clip
    {
        get => audioSource.clip;
        set { audioSource.clip = value; }
    }
    public class AttackInfo
    {
        public bool hasComplete;
        public Coroutine coroutine;
        public UnityAction completeEvent;

        public AttackInfo()
        {
            hasComplete = false;
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(OnPlay != null)
        {
            if (OnPlay.coroutine != null)
                StopCoroutine(OnPlay.coroutine);
            if (!OnPlay.hasComplete)
                OnPlay.completeEvent?.Invoke();
            OnPlay = null;
        }

        var audioAttack = new AttackInfo();
        var completeAction = new UnityAction(OnCompleteEvent);
        var timer = StartCoroutine(attckTimer(audioAttack));
        audioAttack.completeEvent += completeAction;
        audioAttack.coroutine = timer;

        audioSource.Play();
    }

    public void DestoryData()
    {
        audioSource.clip = null;
        audioSource.Stop();
        OnPlay = null;
    }

    IEnumerator attckTimer(AttackInfo _attackInfo)
    {
        float leng = audioSource.clip.length;

        yield return new WaitForSeconds(leng);

        _attackInfo.completeEvent?.Invoke();
        _attackInfo.hasComplete = true;
    }

    public AudioSourceAttach OnComplete(UnityAction _callback)
    {
        OnCompleteEvent += _callback;
        return this;
    }
}
