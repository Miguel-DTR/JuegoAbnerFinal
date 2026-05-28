
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoCinematica : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string escenaJuego;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;

        videoPlayer.Play();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LoadGameScene();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(escenaJuego);
    }
}
