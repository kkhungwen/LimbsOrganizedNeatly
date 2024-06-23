using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    [SerializeField] private AudioMixerGroup musicMasterMixerGroup;
    [SerializeField] private AudioMixerSnapshot musicOffSnapShot;
    [SerializeField] private AudioMixerSnapshot musicLowSnapShot;
    [SerializeField] private AudioMixerSnapshot musicOnFullSnapShot;

    private AudioSource musicAudioSource;
    private AudioClip currentAudioClip;
    private Coroutine fadeOutMusicCoroutine;
    private Coroutine fadeInMusicCoroutine;
    public int musicVolume = 0;
    private const float musicFadeOutTime = 1f;
    private const float musicFadeInTime = 1f;

    protected override void Awake()
    {
        base.Awake();

        musicAudioSource = GetComponent<AudioSource>();

        musicOffSnapShot.TransitionTo(0f);
    }

    private void Start()
    {
        SetMusicVolume(musicVolume);
    }

    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = musicFadeOutTime, float fadeInTime = musicFadeInTime)
    {
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeOutTime, fadeInTime));
    }

    private IEnumerator PlayMusicRoutine(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {
        if (fadeOutMusicCoroutine != null)
            StopCoroutine(fadeOutMusicCoroutine);

        if (fadeInMusicCoroutine != null)
            StopCoroutine(fadeInMusicCoroutine);

        if (musicTrack.musicClip != currentAudioClip)
        {
            currentAudioClip = musicTrack.musicClip;

            yield return fadeOutMusicCoroutine = StartCoroutine(FadeOutMusic(fadeOutTime));

            yield return fadeInMusicCoroutine = StartCoroutine(FadeInMusic(musicTrack, fadeInTime));
        }
        yield return null;
    }

    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        musicLowSnapShot.TransitionTo(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime);
    }

    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {
        musicAudioSource.clip = musicTrack.musicClip;
        musicAudioSource.volume = musicTrack.musicVolme;
        musicAudioSource.Play();

        musicOnFullSnapShot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    private void SetMusicVolume(int musicVolume)
    {
        float muteDecibels = -80f;
        if (musicVolume == 0)
        {
            musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", muteDecibels);
        }
        else
        {
            musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", HelperUtils.LinearToDecibels(musicVolume));
        }
    }
}
