using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundEffectSO soundEffect)
    {
        audioSource.pitch = Random.Range(soundEffect.pitchRandomMin, soundEffect.pitchRandomMax);
        audioSource.volume = soundEffect.volume;
        audioSource.clip = soundEffect.spundEffectClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
