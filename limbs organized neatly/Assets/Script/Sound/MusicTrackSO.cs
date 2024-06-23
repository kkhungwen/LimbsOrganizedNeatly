using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MusicTrackSO_", menuName ="Scriptable Objects/Music Track")]
public class MusicTrackSO : ScriptableObject
{
    public AudioClip musicClip;
    public float musicVolme = 1f;
}
