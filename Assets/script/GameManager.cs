using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int lifes = 3;
    public int points = 0;
    public Block[] blocks;
    public int blockCount = 0;

    [Header("UI")]
    public TMP_Text pointText;
    public TMP_Text lifesText;

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
        if (pointText != null) pointText.text = $"Puntos: {points}";
        if (lifesText != null) lifesText.text = $"Vidas: {lifes}";
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

    public void LoseLifes()
    {
        lifes--;
        if (lifes <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) player.SetActive(false);

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }
}