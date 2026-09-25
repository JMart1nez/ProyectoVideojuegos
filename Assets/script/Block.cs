using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Power-Up / Cápsula")]
    public GameObject capsule;

    private void Start()
    {
        // Asignación de color aleatorio al iniciar
        Renderer blockRenderer = GetComponent<Renderer>();
        if (blockRenderer != null)
        {
            Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            blockRenderer.material.color = randomColor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (capsule != null)
            {
                int posibility = Random.Range(0, 10);
                if (posibility == 5)
                {
                    // Comprobamos si el GameManager está en Modo Individual
                    bool isSoloMode = GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo;

                    if (isSoloMode)
                    {
                        // --- MODO INDIVIDUAL (Arriba / Abajo) ---
                        
                        GameObject capDown = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptDown = capDown.GetComponent<Capsule>();
                        if (capScriptDown != null)
                        {
                            capScriptDown.moveDirection = Vector3.down;
                        }

                        GameObject capUp = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptUp = capUp.GetComponent<Capsule>();
                        if (capScriptUp != null)
                        {
                            capScriptUp.moveDirection = Vector3.up;
                        }
                    }
                    else
                    {
                        // --- MODO CO-OP / VERSUS (Izquierda / Derecha) ---
                        
                        GameObject capLeft = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptLeft = capLeft.GetComponent<Capsule>();
                        if (capScriptLeft != null)
                        {
                            capScriptLeft.moveDirection = Vector3.left;
                        }

                        GameObject capRight = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptRight = capRight.GetComponent<Capsule>();
                        if (capScriptRight != null)
                        {
                            capScriptRight.moveDirection = Vector3.right;
                        }
                    }
                }
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.BlockDestroy();
            }

            Destroy(gameObject);
        }
    }
}