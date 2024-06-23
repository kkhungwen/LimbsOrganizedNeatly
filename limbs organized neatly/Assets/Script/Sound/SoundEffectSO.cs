using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SoundEffectSO_",menuName ="Scriptable Objects/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    public AudioClip spundEffectClip;

    [Range(0.1f, 1.5f)]
    public float pitchRandomMin = 0.8f;
    [Range(0.1f, 1.5f)]
    public float pitchRandomMax = 1.2f;
    [Range(0f, 1f)]
    public float volume = 1f;
}
