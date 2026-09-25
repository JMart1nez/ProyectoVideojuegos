using UnityEngine;
using TMPro;

// Declaración del enum para los modos de juego
public enum GameMode { Solo, CoOp, Versus }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuración de Modo")]
    public GameMode currentMode = GameMode.Solo;

    [Header("Estadísticas Jugador 1")]
    public int lifes = 3;
    public int points = 0;

    [Header("Estadísticas Jugador 2")]
    public int lifesP2 = 3;
    public int pointsP2 = 0;

    [Header("Control de Bloques")]
    public Block[] blocks;
    public int blockCount = 0;

    [Header("UI Jugador 1")]
    public TMP_Text pointText;
    public TMP_Text lifesText;

    [Header("UI Jugador 2")]
    public TMP_Text pointTextP2;
    public TMP_Text lifesTextP2;

    [Header("UI de Game Over")]
    public GameObject gameOverCanvas;
    public TMP_Text textoGanador;
    public TMP_Text textoDetalle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        blockCount = blocks.Length;
    }

    void Update()
    {
        // Actualiza interfaz Jugador 1
        if (pointText != null) pointText.text = $"Puntos P1: {points}";
        if (lifesText != null) lifesText.text = $"Vidas P1: {lifes}";

        // Actualiza interfaz Jugador 2 si sus textos están asignados
        if (pointTextP2 != null) pointTextP2.text = $"Puntos P2: {pointsP2}";
        if (lifesTextP2 != null) lifesTextP2.text = $"Vidas P2: {lifesP2}";
    }

    // === NUEVOS MÉTODOS PARA SUMAR PUNTOS POR JUGADOR ===
    public void AddPointsP1(int amount)
    {
        points += amount;
    }

    public void AddPointsP2(int amount)
    {
        pointsP2 += amount;
    }

    // ====================================================

    public void BlockDestroy()
    {
        blockCount--;
        // ⚠️ Ya NO sumamos puntos aquí.
        // Los puntos se suman desde Block.cs según qué pelota rompió el bloque.

        if (blockCount <= 0)
        {
            EndGame();
        }
    }

    // Métodos para restar vidas al Jugador 1
    public void LoseLifeP1()
    {
        lifes--;
        if (lifes <= 0)
        {
            EndGame();
        }
    }

    public void LoseLifes() => LoseLifeP1(); 

    // Métodos para restar vidas al Jugador 2
    public void LoseLifeP2()
    {
        lifesP2--;
        if (lifesP2 <= 0)
        {
            EndGame();
        }
    }

    public void LoseLifesP2() => LoseLifeP2(); 
    public void EndGame()
    {
        // Desactiva jugadores y pelotas
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }

        // === MOSTRAR PANTALLA DE GAME OVER ===
        string mensajeGanador = "";
        string mensajeDetalle = "";

        switch (currentMode)
        {
            case GameMode.Solo:
                mensajeGanador = "¡NIVEL COMPLETADO!";
                mensajeDetalle = $"Puntos: {points}";
                break;

            case GameMode.CoOp:
                mensajeGanador = "¡NIVEL COMPLETADO!";
                mensajeDetalle = $"Puntos totales: {points}";
                break;

            case GameMode.Versus:
                // Determinar ganador por vidas o por puntos
                if (lifes <= 0 && lifesP2 > 0)
                {
                    mensajeGanador = "¡JUGADOR 2 GANA!";
                }
                else if (lifesP2 <= 0 && lifes > 0)
                {
                    mensajeGanador = "¡JUGADOR 1 GANA!";
                }
                else if (lifes <= 0 && lifesP2 <= 0)
                {
                    mensajeGanador = "¡EMPATE!";
                }
                else
                {
                    // Se acabaron los bloques: gana quien tenga más puntos
                    if (points > pointsP2)
                        mensajeGanador = "¡JUGADOR 1 GANA!";
                    else if (pointsP2 > points)
                        mensajeGanador = "¡JUGADOR 2 GANA!";
                    else
                        mensajeGanador = "¡EMPATE!";
                }

                mensajeDetalle = $"P1: {points} pts ({lifes} vidas)  |  P2: {pointsP2} pts ({lifesP2} vidas)";
                break;
        }

        Debug.Log("=== FIN DEL JUEGO ===");
        Debug.Log(mensajeGanador);
        Debug.Log(mensajeDetalle);

        // Activar el Canvas y poner los textos
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        if (textoGanador != null)
        {
            textoGanador.text = mensajeGanador;
        }

        if (textoDetalle != null)
        {
            textoDetalle.text = mensajeDetalle;
        }
    }
}