using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")]
    [SerializeField] Canvas mainMenuCanvas;

    [Header("==== BUTTONS ====")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;

    void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();
    }

    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}