using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject capsule;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();

            if (ball != null && GameManager.Instance != null)
            {
                // === SOLO y COOP: PUNTOS COMPARTIDOS (P1) ===
                if (GameManager.Instance.currentMode == GameMode.Solo ||
                    GameManager.Instance.currentMode == GameMode.CoOp)
                {
                    GameManager.Instance.AddPointsP1(100);
                }
                // === VERSUS: PUNTOS SEPARADOS ===
                else if (GameManager.Instance.currentMode == GameMode.Versus)
                {
                    if (ball.startingSide == Ball.PlayerSide.Left)
                        GameManager.Instance.AddPointsP1(100);
                    else
                        GameManager.Instance.AddPointsP2(100);
                }
            }

            // === CÁPSULAS DE PODER ===
            int posibility = Random.Range(0, 10);
            if (posibility == 5)
            {
                GameObject capLeft = Instantiate(capsule, transform.position, Quaternion.identity);
                Capsule capScriptLeft = capLeft.GetComponent<Capsule>();
                if (capScriptLeft != null)
                    capScriptLeft.moveDirection = Vector3.left;

                GameObject capRight = Instantiate(capsule, transform.position, Quaternion.identity);
                Capsule capScriptRight = capRight.GetComponent<Capsule>();
                if (capScriptRight != null)
                    capScriptRight.moveDirection = Vector3.right;
            }

            // === DESTRUIR EL BLOQUE ===
            if (GameManager.Instance != null)
                GameManager.Instance.BlockDestroy();

            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Renderer>().material.color = randomColor;
    }
}