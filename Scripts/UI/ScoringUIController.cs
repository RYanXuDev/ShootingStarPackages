using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("==== BACKGROUND ====")]
    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundImages;

    [Header("==== SCORING SCREEN ====")]
    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;

    void Start()
    {
        ShowRandomBackground();
        ShowScoringScreen();

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);

        GameManager.GameState = GameState.Scoring;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // TODO: Update high score leaderboard UI
    }

    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}