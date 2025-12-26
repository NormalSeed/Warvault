using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button endButton;
    [SerializeField] Button closeButton;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        endButton.onClick.AddListener(OnEndButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    void OnContinueButtonClicked()
    {
        UIManager.Instance.isMenuOpen.Value = false;
    }

    void OnRestartButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        UIManager.Instance.isMenuOpen.Value = false;
    }

    void OnEndButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnCloseButtonClicked()
    {
        UIManager.Instance.isMenuOpen.Value = false;
    }
}
