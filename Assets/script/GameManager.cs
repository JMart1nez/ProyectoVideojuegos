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

    public void BlockDestroy()
    {
        blockCount--;
        points += 100;
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
    }
}