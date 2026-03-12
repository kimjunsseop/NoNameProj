using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;

    void OnEnable()
    {
        GameEvents.OnPlayerDead += Open;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDead -= Open;
    }

    void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
    }
}