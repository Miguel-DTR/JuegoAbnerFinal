using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaBotton : MonoBehaviour
{
    [SerializeField] GameObject pausaPanel;
    [SerializeField] string escenaName;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickAudio;
    public void Pause()
    {
        audioSource.PlayOneShot(clickAudio);
        pausaPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        audioSource.PlayOneShot(clickAudio);
        pausaPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CargarEscena()
    {
        audioSource.PlayOneShot(clickAudio);
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaName);
    }
}
