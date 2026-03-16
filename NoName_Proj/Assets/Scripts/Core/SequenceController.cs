using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SequenceController : MonoBehaviour
{
    public PlayableDirector introTimeline;
    public GameObject button;

    void Start()
    {
        introTimeline.stopped += OnIntroFinished;
        introTimeline.Play();
    }

    void OnIntroFinished(PlayableDirector dir)
    {
        button.SetActive(true);
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("StageBoss");
    }
}