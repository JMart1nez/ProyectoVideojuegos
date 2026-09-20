using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartSoloGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentMode = GameMode.Solo;
        
        SceneManager.LoadScene("Juego");
    }

    public void StartCoOpGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentMode = GameMode.CoOp;

        SceneManager.LoadScene("Juego");
    }

    public void StartVersusGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentMode = GameMode.Versus;

        SceneManager.LoadScene("Juego");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}