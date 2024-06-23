using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitButton;

    protected override void Awake()
    {
        base.Awake();

        startGameButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        SceneControllerManager.Instance.FadeAndLoadScene("GamePlay");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
