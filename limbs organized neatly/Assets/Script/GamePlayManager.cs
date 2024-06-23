using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [SerializeField] private MusicTrackSO musicTrackSO;
    [SerializeField] private LevelSO[] levelArray;
    [SerializeField] private FadeIn fadeIn;
    private int levelCount;

    private void Start()
    {
        LevelManager.Instance.OnLevelClear += Instance_OnLevelClear;
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        levelCount = 0;

        yield return StartCoroutine(fadeIn.FadeInText("After a day of hardwork, it's time to pack and leave", 4));

        LevelManager.Instance.StartCoroutine(LevelManager.Instance.LoadLevel(levelArray[levelCount]));
        MusicManager.Instance.PlayMusic(musicTrackSO);
        //SoundManager.Instance.PlayAudio(AudioEnum.bgm);
    }

    private void Instance_OnLevelClear()
    {
        levelCount++;

        if (levelCount >= levelArray.Length)
        {
            SceneControllerManager.Instance.FadeAndLoadScene("Menu");
            SoundManager.Instance.StopBGM();
            return;
        }

        LevelManager.Instance.StartCoroutine(LevelManager.Instance.LoadLevel(levelArray[levelCount]));
    }
}
