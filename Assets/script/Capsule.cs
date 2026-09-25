using System.Collections;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public float speed = 3f;
    public GameObject prefabBall;
    public int type = 0;

    private Transform currentBall;

    private bool isCollected = false;

    // Direccion de movimiento personalizada
    [HideInInspector] public Vector3 moveDirection = Vector3.down;

    void Start()
    {
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj != null) currentBall = ballObj.transform;
        
        // 0, 1, 4: Poderes | 2, 3, 5: Desventajas
        type = Random.Range(0, 6);                         
    }

    void Update()
    {
        if (isCollected) return;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        if (transform.position.y < -8f || Mathf.Abs(transform.position.x) > 22f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
            return;
        }
        
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;

            float tiempoDeDestruccion = 0.1f;

            switch (type)
            {
                case 0: // Poder: Multibola
                    MultiBall();
                    tiempoDeDestruccion = 0.1f;
                    break;
                case 1: // Poder: Agrandar jugador (Duración 5s)
                    StartCoroutine(ExtendPlayer(other.transform, 5f));
                    tiempoDeDestruccion = 5.5f;                    
                    break;
                case 2: // Desventaja: Pelota veloz (Duración 4s)
                    StartCoroutine(ExtraSpeed());
                    tiempoDeDestruccion = 4.5f;
                    break;
                case 3: // Desventaja: Reducir jugador (Duración 5s)
                    StartCoroutine(ClipPlayer(other.transform, 5f));
                    tiempoDeDestruccion = 5.5f;                    
                    break;
                case 4: // Poder Novedoso: Imán Repulsor Temporal (Duración 6s)
                    StartCoroutine(RepulsorMagnet(other.transform, 6f, 3.5f));
                    tiempoDeDestruccion = 6.5f;
                    break;
                case 5: // Desventaja: Perder una vida
                    TakeDamage(other.gameObject);
                    tiempoDeDestruccion = 0.1f;
                    break;
            }

            Destroy(gameObject, tiempoDeDestruccion);
        }
    }

    // MULTIBOLA
    void MultiBall()
    {
        if (currentBall == null) return;

        for (int i = 0; i < 2; i++)
        {
            Vector3 offset = new Vector3((i == 0 ? 0.8f : -0.8f), 0.5f, 0f);
            GameObject newBall = Instantiate(prefabBall, currentBall.position + offset, Quaternion.identity);
            
            Ball ballScript = newBall.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.Launch();
            }
        }
    }

    IEnumerator ExtendPlayer(Transform player, float duration)
    {
        player.localScale = new Vector3(5f, 0.3f, 1f);
        yield return new WaitForSeconds(duration);
        player.localScale = new Vector3(3f, 0.3f, 1f);
    }

    IEnumerator ClipPlayer(Transform player, float duration)
    {
        player.localScale = new Vector3(1.5f, 0.3f, 1f);
        yield return new WaitForSeconds(duration);
        player.localScale = new Vector3(3f, 0.3f, 1f);
    }

    IEnumerator ExtraSpeed()
    {
        if (currentBall != null)
        {
            Ball b = currentBall.GetComponent<Ball>();
            if (b != null)
            {
                if (b.launchSpeed < 12f) 
                {
                    b.MultiplySpeed(1.5f);
                    yield return new WaitForSeconds(4f);
                    b.MultiplySpeed(1f / 1.5f);
                }
            }
        }
    }

    // IMÁN REPULSOR TEMPORAL 
    IEnumerator RepulsorMagnet(Transform player, float duration, float magnetRadius)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // Busca todas las pelotas activas en la escena
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (GameObject b in balls)
            {
                if (b != null)
                {
                    float distance = Vector3.Distance(player.position, b.transform.position);

                    // Si la pelota entra en el radio de protección de la paleta, la repele hacia arriba
                    if (distance < magnetRadius)
                    {
                        Rigidbody ballRb = b.GetComponent<Rigidbody>();
                        if (ballRb != null)
                        {
                            Vector3 pushDirection = (b.transform.position - player.position).normalized;
                            if (pushDirection.y < 0.2f) pushDirection.y = 0.8f; 

                            ballRb.linearVelocity = pushDirection.normalized * ballRb.linearVelocity.magnitude;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    // DESVENTAJA: PERDER UNA VIDA 
    void TakeDamage(GameObject playerObj)
    {
        if (GameManager.Instance != null)
        {
            if (playerObj.name.Contains("2"))
            {
                GameManager.Instance.LoseLifeP2();
            }
            else
            {
                GameManager.Instance.LoseLifeP1();
            }
        }
    }
}