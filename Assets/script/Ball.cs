using UnityEngine;

public class Ball : MonoBehaviour
{
    public float launchSpeed = 8f;
    public Transform paddle;
    public Vector3 offset;    

    private Rigidbody rb;
    private bool isLaunched = false;
    private bool isProcessingDeath = false;

    public enum PlayerSide { Left, Right }
    [Header("Configuración de Inicio")]
    public PlayerSide startingSide;
    public KeyCode launchKey;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if(GameManager.Instance.currentMode == GameMode.Solo){
            offset = new Vector3(0, 0.75f, 0);
        } else {
            if (startingSide == PlayerSide.Left)
                offset = new Vector3(0.75f, 0, 0);
            else
                offset = new Vector3(-0.75f, 0, 0);
        }
        if (!isLaunched)
        {
            ResetBall();
        }
    }

    private void Update()
    {
        if (!isLaunched)
        {
            FollowPaddle();
            if (Input.GetKeyDown(launchKey))
            {
                Launch();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    void FixedUpdate()
    {
        if (!isLaunched) return;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            isLaunched = false; 
            Launch();
            return;
        }

        rb.linearVelocity = rb.linearVelocity.normalized * launchSpeed;

        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo)
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 1.5f)
            {
                float yDir = (rb.linearVelocity.y >= 0) ? 1.5f : -1.5f;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, yDir, 0f).normalized * launchSpeed;
            }
        }
        else
        {
            if (Mathf.Abs(rb.linearVelocity.x) < 1.5f)
            {
                float xDirection = (rb.linearVelocity.x >= 0) ? 1.5f : -1.5f;
                rb.linearVelocity = new Vector3(xDirection, rb.linearVelocity.y, 0f).normalized * launchSpeed;
            }
        }

        if (Mathf.Abs(transform.position.x) > 20f || Mathf.Abs(transform.position.y) > 20f)
        {
            if (!isProcessingDeath)
            {
                GameObject deadZone = GameObject.FindGameObjectWithTag("DeadZone");
                if (deadZone != null)
                {
                    OnTriggerEnter(deadZone.GetComponent<Collider>());
                }
                else if (paddle == null)
                {
                    Destroy(gameObject); // Si no encuentra DeadZone y es clon, forzar destrucción
                }
            }
        }
    }

    void FollowPaddle()
    {
        if (paddle != null)
        {
            transform.position = paddle.position + offset;
        }
    }

    public void Launch()
    {
        if (isLaunched) return;

        isLaunched = true;
        float angle;

        // Ajuste de ángulo según el modo
        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo)
        {
            // En modo Individual sale disparada hacia arriba
            angle = Random.Range(45f, 135f);
        }
        else
        {
            // En CoOp / Versus sale a los lados
            angle = (startingSide == PlayerSide.Left) ? Random.Range(-45f, 45f) : Random.Range(135f, 225f);
        }

        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        rb.linearVelocity = direction.normalized * launchSpeed;
    }
    
    public void ResetBall()
    {
        isLaunched = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        FollowPaddle();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            if (isProcessingDeath) return;

            // 1. Identificar si es un CLON de Multibola
            // Los clones se instancian desde un Prefab, por lo que su 'paddle' está vacío.
            if (paddle == null)
            {
                Destroy(gameObject);
                return; 
            }

            // 2. Si llegamos aquí, es una PELOTA ORIGINAL
            isProcessingDeath = true;
            bool isSolo = GameManager.Instance != null && (GameManager.Instance.currentMode == GameMode.Solo || GameManager.Instance.currentMode == GameMode.CoOp);

            if (isSolo)
            {
                GameManager.Instance.LoseLifes();
            }
            else
            {
                // Saber de qué lado del mapa cruzó la pelota (Izquierda = P1, Derecha = P2)
                if (transform.position.x < 0)
                {
                    GameManager.Instance.LoseLifes(); // Falla P1
                }
                else
                {
                    GameManager.Instance.LoseLifesP2(); // Falla P2
                }
            }

            // 3. Regresar la pelota original a la paleta de su dueño
            ResetBall();
            Invoke(nameof(ReactivateDeath), 0.5f);
        }
    }

    // Método auxiliar
    void ReactivateDeath()
    {
        isProcessingDeath = false;
    }

    public void MultiplySpeed(float multi)
    {
        launchSpeed *= multi;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Determina si el pad es vertical (Pong) u horizontal (Breakout)
            bool isVerticalPaddle = collision.collider.bounds.size.y > collision.collider.bounds.size.x;

            if (isVerticalPaddle)
            {
                // Rebote para modo Co-Op / Versus (Izquierda - Derecha)
                float offsetY = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
                float directionX = (transform.position.x > collision.transform.position.x) ? 1f : -1f;
                Vector3 newDirection = new Vector3(directionX, offsetY * 2.5f, 0f).normalized;
                rb.linearVelocity = newDirection * launchSpeed;
            }
            else
            {
                // Rebote para modo Individual (Arriba - Abajo)
                float offsetX = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
                
                // CORRECCIÓN: Verifica si la pelota está arriba o abajo del pad para rebotar en la dirección correcta
                float directionY = (transform.position.y > collision.transform.position.y) ? 1f : -1f;
                
                Vector3 newDirection = new Vector3(offsetX * 2.5f, directionY, 0f).normalized;
                rb.linearVelocity = newDirection * launchSpeed;
            }
        }
        else
        {
            Vector3 randomTweak = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
            rb.linearVelocity = (rb.linearVelocity + randomTweak).normalized * launchSpeed;
        }
    }
}