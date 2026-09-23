using UnityEngine;

public class Ball : MonoBehaviour
{
    public float launchSpeed = 8f;
    public Transform paddle;
    public Vector3 offset;

    private Rigidbody rb;
    private bool isLaunched = false;

    // Define de que lado esta asignada esta pelota
    public enum PlayerSide { Left, Right }
    [Header("Configuración de Inicio")]
    public PlayerSide startingSide;
    // Tecla para que el jugador lance la pelota
    public KeyCode launchKey;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if(startingSide == PlayerSide.Left)
        {
            offset = new Vector3(0.75f, 0, 0);        
        } else {
            offset = new Vector3(-0.75f, 0, 0);        
        }   
        ResetBall();
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

        // Mantener la velocidad constante
        rb.linearVelocity = rb.linearVelocity.normalized * launchSpeed;

        // Validar el modo de juego para evitar el atasco correcto
        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo)
        {
            // Evitar trayectoria vertical pura
            if (Mathf.Abs(rb.linearVelocity.x) < 2f)
            {
                float xDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
                rb.linearVelocity = new Vector3(xDirection * 2f, rb.linearVelocity.y, 0f).normalized * launchSpeed;
            }
        }
        else 
        {
            // Evitar trayectoria horizontal
            if (Mathf.Abs(rb.linearVelocity.y) < 1.5f)
            {
                float yDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, yDirection * 1.5f, 0f).normalized * launchSpeed;
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
        float angle = Random.Range(-60f, 60f);
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
            // Revisa cuántas pelotas hay en la escena
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            
            if (balls.Length > 2) 
            {
                // Si hay más de una destruye esta copia
                Destroy(gameObject);
            }
            else
            {
                // Si es la última pelota que queda resta vidas
                GameManager.Instance.LoseLifes();
                ResetBall();
            }
        }
    }

    public void MultiplySpeed(float multi)
    {
        launchSpeed *= multi;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            // Compara las dimensiones para saber si la paleta está en vertical u horizontal
            bool isVerticalPaddle = collision.collider.bounds.size.y > collision.collider.bounds.size.x;

            if (isVerticalPaddle)
            {
                // Rebote para MODO PONG 
                float offsetY = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
                float directionX = (transform.position.x > collision.transform.position.x) ? 1f : -1f;
                Vector3 newDirection = new Vector3(directionX, offsetY * 2.5f, 0f).normalized;
                rb.linearVelocity = newDirection * launchSpeed;
            }
            else
            {
                // Rebote para MODO CLÁSICO
                float offsetX = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
                Vector3 newDirection = new Vector3(offsetX * 2.5f, 1f, 0f).normalized;
                rb.linearVelocity = newDirection * launchSpeed;
            }
        }
        else
        {
            // Romper bucles al chocar con paredes o bloques
            Vector3 randomTweak = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
            rb.linearVelocity = (rb.linearVelocity + randomTweak).normalized * launchSpeed;
        }
    }
}