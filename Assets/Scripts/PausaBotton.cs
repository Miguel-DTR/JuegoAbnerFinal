using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaBotton : MonoBehaviour
{
    [SerializeField] GameObject pausaPanel;
    public void Pause()
    {
        pausaPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        pausaPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CargarEscena(string escena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escena);
    }
}
