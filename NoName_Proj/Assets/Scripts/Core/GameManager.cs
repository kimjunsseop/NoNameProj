using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameEvents.OnStageClear += OnStageClear;
        GameEvents.OnNextStage += LoadNextStage;
    }

    void OnDisable()
    {
        GameEvents.OnStageClear -= OnStageClear;
        GameEvents.OnNextStage -= LoadNextStage;
    }

    void OnStageClear()
    {
        Time.timeScale = 0f;

        GameEvents.OnOpenUpgradeUI?.Invoke();
    }

    void LoadNextStage()
    {
        Time.timeScale = 1f;

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(nextScene);
    }
}