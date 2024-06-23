using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioEnum
{
    bgm,
    click,
    drop,
    spin
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private Queue<AudioSource> audioSourceQueue;
    private AudioSource bgmAudioSource;

    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip drop;
    [SerializeField] private AudioClip spin;

    private int audioSourceAmount = 10;

    protected override void Awake()
    {
        base.Awake();

        audioSourceQueue = new Queue<AudioSource>();

        for (int i = 0; i < audioSourceAmount; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceQueue.Enqueue(audioSource);
        }

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayAudio(AudioEnum audio)
    {
        switch (audio)
        {
            case AudioEnum.bgm:
                PlayBGMAudio(bgm);
                break;

            case AudioEnum.click:
                PlayAudioOnce(click);
                break;

            case AudioEnum.drop:
                PlayAudioOnce(drop);
                break;

            case AudioEnum.spin:
                PlayAudioOnce(spin);
                break;
        }
    }

    private void PlayAudioOnce(AudioClip clip)
    {
        AudioSource audioSource = audioSourceQueue.Dequeue();
        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.Play();
        audioSourceQueue.Enqueue(audioSource);
    }

    private void PlayBGMAudio(AudioClip clip)
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
}
