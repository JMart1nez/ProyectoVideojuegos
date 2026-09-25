using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Power-Up / Cápsula")]
    public GameObject capsule;

    private void Start()
    {
        // Asignación de color aleatorio al iniciar con validación de seguridad
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
            // 1. Lógica de Puntuación (Versión de tu compañero)
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null && GameManager.Instance != null)
            {
                if (GameManager.Instance.currentMode == GameMode.Solo || GameManager.Instance.currentMode == GameMode.CoOp)
                {
                    GameManager.Instance.AddPointsP1(100);
                }
                else if (GameManager.Instance.currentMode == GameMode.Versus)
                {
                    if (ball.startingSide == Ball.PlayerSide.Left)
                        GameManager.Instance.AddPointsP1(100);
                    else
                        GameManager.Instance.AddPointsP2(100);
                }
            }

            // 2. Lógica de Cápsulas (Tu versión adaptativa)
            if (capsule != null)
            {
                int posibility = Random.Range(0, 10);
                if (posibility == 5)
                {
                    bool isSoloMode = GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo;

                    if (isSoloMode)
                    {
                        // Modo Individual: Arriba / Abajo
                        GameObject capDown = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptDown = capDown.GetComponent<Capsule>();
                        if (capScriptDown != null) capScriptDown.moveDirection = Vector3.down;

                        GameObject capUp = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptUp = capUp.GetComponent<Capsule>();
                        if (capScriptUp != null) capScriptUp.moveDirection = Vector3.up;
                    }
                    else
                    {
                        // Modo Co-op / Versus: Izquierda / Derecha
                        GameObject capLeft = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptLeft = capLeft.GetComponent<Capsule>();
                        if (capScriptLeft != null) capScriptLeft.moveDirection = Vector3.left;

                        GameObject capRight = Instantiate(capsule, transform.position, Quaternion.identity);
                        Capsule capScriptRight = capRight.GetComponent<Capsule>();
                        if (capScriptRight != null) capScriptRight.moveDirection = Vector3.right;
                    }
                }
            }

            // 3. Destrucción del bloque
            if (GameManager.Instance != null)
            {
                GameManager.Instance.BlockDestroy();
            }

            Destroy(gameObject);
        }
    }

}