using UnityEngine;
using TMPro;

public class LevelSetup : MonoBehaviour
{
    [Header("Objetos de Jugadores")]
    public GameObject player1Paddle;
    public GameObject player2Paddle;

    [Header("Pelotas")]
    public GameObject ballP1;
    public GameObject ballP2;

    [Header("Grupos de UI")]
    public GameObject uiPlayer1Group;
    public GameObject uiPlayer2Group;

    [Header("Referencias de Texto P2")]
    public TMP_Text pointTextP2;
    public TMP_Text lifesTextP2;

    void Start()
    {
        GameMode mode = GameMode.Solo;

        if (GameManager.Instance != null)
        {
            mode = GameManager.Instance.currentMode;

            // === RESETEAR ESTADÍSTICAS AL ENTRAR A UN NUEVO NIVEL ===
            GameManager.Instance.lifes = 3;
            GameManager.Instance.lifesP2 = 3;
            GameManager.Instance.points = 0;
            GameManager.Instance.pointsP2 = 0;

            // Conecta las referencias de UI de P2 directamente al Singleton
            if (pointTextP2 != null) GameManager.Instance.pointTextP2 = pointTextP2;
            if (lifesTextP2 != null) GameManager.Instance.lifesTextP2 = lifesTextP2;
        }

        // Desactivar GameOverCanvas al iniciar el nivel
        if (GameManager.Instance != null && GameManager.Instance.gameOverCanvas != null)
        {
            GameManager.Instance.gameOverCanvas.SetActive(false);
        }

        ConfigureLevel(mode);
    }

    void ConfigureLevel(GameMode mode)
    {
        Debug.Log("Modo configurado en LevelSetup: " + mode);

        switch (mode)
        {
            case GameMode.Solo:
                player1Paddle.SetActive(true);
                if (ballP1 != null) ballP1.SetActive(true);
                if (player2Paddle != null) player2Paddle.SetActive(false);
                if (ballP2 != null) ballP2.SetActive(false);
                if (uiPlayer1Group != null) uiPlayer1Group.SetActive(true);
                if (uiPlayer2Group != null) uiPlayer2Group.SetActive(false);
                break;

            case GameMode.CoOp:
                player1Paddle.SetActive(true);
                if (ballP1 != null) ballP1.SetActive(true);
                if (player2Paddle != null) player2Paddle.SetActive(true);
                if (ballP2 != null) ballP2.SetActive(true);
                if (uiPlayer1Group != null) uiPlayer1Group.SetActive(true);
                if (uiPlayer2Group != null) uiPlayer2Group.SetActive(true);
                break;

            case GameMode.Versus:
                player1Paddle.SetActive(true);
                if (player2Paddle != null) player2Paddle.SetActive(true);
                if (ballP1 != null) ballP1.SetActive(true);
                if (ballP2 != null) ballP2.SetActive(true);
                if (uiPlayer1Group != null) uiPlayer1Group.SetActive(true);
                if (uiPlayer2Group != null) uiPlayer2Group.SetActive(true);
                break;
        }
    }
}