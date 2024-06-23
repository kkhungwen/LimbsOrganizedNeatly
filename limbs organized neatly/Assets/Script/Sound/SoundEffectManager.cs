using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager>
{
    public int soundVolume = 8;
    public AudioMixerGroup soundMasterMixerGroup;
    private Queue<SoundEffect> soundEffectQueue;
    private int soundEffectCount = 20;

    private void Start()
    {
        SetSoundVolume(soundVolume);
        CreateSoundEffectQueue();
    }

    public void PlaySoundEffect(SoundEffectSO soundEffectSO)
    {
        SoundEffect soundEffect = soundEffectQueue.Dequeue();
        soundEffect.PlaySound(soundEffectSO);
        soundEffectQueue.Enqueue(soundEffect);
    }

    private void SetSoundVolume(int soundVolume)
    {
        float muteDecibels = -80f;

        if (soundVolume == 0)
        {
            soundMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            soundMasterMixerGroup.audioMixer.SetFloat("soundsVolume", HelperUtils.LinearToDecibels(soundVolume));
        }
    }

    private void CreateSoundEffectQueue()
    {
        soundEffectQueue = new();

        for (int i = 0; i < soundEffectCount; i++)
        {
            GameObject gameObject = new("SoundEffect " + i);
            gameObject.transform.parent = transform;
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            SoundEffect soundEffect = gameObject.AddComponent<SoundEffect>();
            audioSource.outputAudioMixerGroup = soundMasterMixerGroup;

            soundEffectQueue.Enqueue(soundEffect);
        }
    }
}
