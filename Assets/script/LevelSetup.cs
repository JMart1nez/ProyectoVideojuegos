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

    [Header("Referencias de Texto P1")]
    public TMP_Text pointTextP1;
    public TMP_Text lifesTextP1;

    [Header("Referencias de Texto P2")]
    public TMP_Text pointTextP2;
    public TMP_Text lifesTextP2;

    [Header("Game Over")]
    public GameObject gameOverCanvas;
    public TMP_Text textoGanador;
    public TMP_Text textoDetalle;

    void Start()
    {
        GameMode mode = GameMode.Solo;

        if (GameManager.Instance != null)
        {
            mode = GameManager.Instance.currentMode;

            // Reset de estadísticas al entrar a un nuevo nivel
            GameManager.Instance.lifes = 3;
            GameManager.Instance.lifesP2 = 3;
            GameManager.Instance.points = 0;
            GameManager.Instance.pointsP2 = 0;

            // === REASIGNAR REFERENCIAS DE UI AL SINGLETON ===
            // P1
            if (pointTextP1 != null) GameManager.Instance.pointText = pointTextP1;
            if (lifesTextP1 != null) GameManager.Instance.lifesText = lifesTextP1;
            // P2
            if (pointTextP2 != null) GameManager.Instance.pointTextP2 = pointTextP2;
            if (lifesTextP2 != null) GameManager.Instance.lifesTextP2 = lifesTextP2;
            // Game Over
            if (gameOverCanvas != null) GameManager.Instance.gameOverCanvas = gameOverCanvas;
            if (textoGanador != null) GameManager.Instance.textoGanador = textoGanador;
            if (textoDetalle != null) GameManager.Instance.textoDetalle = textoDetalle;

            // Desactivar Game Over al iniciar
            if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
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
                if (uiPlayer2Group != null) uiPlayer2Group.SetActive(false);
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