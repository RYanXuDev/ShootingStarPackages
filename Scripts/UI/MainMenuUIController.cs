using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] Button buttonStartGame;

    void OnEnable()
    {
        buttonStartGame.onClick.AddListener(OnStartGameButtonClick);
    }

    void OnDisable()
    {
        buttonStartGame.onClick.RemoveAllListeners();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
    }

    void OnStartGameButtonClick()
    {
        SceneLoader.Instance.LoadGameplayScene();
    }
}