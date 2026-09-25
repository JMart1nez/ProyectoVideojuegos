using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("Prefab del Bloque")]
    public GameObject blockPrefab;

    [Header("Tamaño y Escala de los Bloques")]
    public Vector3 blockScale = new Vector3(0.8f, 0.2f, 0.4f); 

    [Header("Dimensiones de la Cuadrícula")]
    public int rows = 15;          // Número de filas
    public int columns = 8;       // Número de columnas
    public Vector2 blockSpacing = new Vector2(2f, 0.8f); // Distancia entre bloques (X, Y)

    [Header("Posición de la Pared (Offset)")]
    public Vector3 centerOffset = new Vector3(0f, 12f, 0f); 

    [Header("Pasillos / Huecos Aleatorios")]
    [Range(0f, 1f)]
    public float emptyChance = 0.35f; 

     void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("¡ERROR! Falta asignar el Block Prefab en el Inspector.");
            return;
        }

        // Elimina bloques anteriores para no encimar
        Block[] existingBlocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        foreach (Block b in existingBlocks)
        {
            Destroy(b.gameObject);
        }

        // Cálculo del punto inicial para centrar la matriz respecto al centerOffset
        float startX = centerOffset.x - ((columns - 1) * blockSpacing.x) / 2f;
        float startY = centerOffset.y - ((rows - 1) * blockSpacing.y) / 2f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (Random.value < emptyChance)
                {
                    continue; // Hueco libre para la pelota
                }

                Vector3 spawnPos = new Vector3(
                    startX + (c * blockSpacing.x),
                    startY + (r * blockSpacing.y),
                    centerOffset.z
                );

                GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, transform);
                
                newBlock.transform.localScale = blockScale;
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
            GameManager.Instance.blockCount = GameManager.Instance.blocks.Length;
        }
    }
}