using System.Collections;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public float speed = 3f;
    public GameObject prefabBall;
    public int type = 0;

    private Transform currentBall;

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
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;

            switch (type)
            {
                case 0: // Poder: Multibola
                    MultiBall();
                    break;
                case 1: // Poder: Agrandar jugador
                    StartCoroutine(ExtendPlayer(other.transform, 5f));                    
                    break;
                case 2: // Desventaja: Pelota veloz
                    StartCoroutine(ExtraSpeed());
                    break;
                case 3: // Desventaja: Reducir jugador
                    StartCoroutine(ClipPlayer(other.transform, 5f));                    
                    break;
                case 4: // Poder Novedoso: Imán Repulsor Temporal
                    StartCoroutine(RepulsorMagnet(other.transform, 6f, 3.5f));
                    break;
                case 5: // Desventaja: Perder una vida
                    TakeDamage(other.gameObject);
                    break;
            }

            Destroy(gameObject, 6.5f);
        }
    }

    // MULTIBOLA
    void MultiBall()
    {
        if (currentBall == null) return;
        
        Vector3 spawnPos = currentBall.position + new Vector3(0.5f, 0f, 0f);

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
                b.MultiplySpeed(1.5f);
                yield return new WaitForSeconds(4f);
                b.MultiplySpeed(1f / 1.5f);
            }
        }
    }

    //  IMÁN REPULSOR TEMPORAL 
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
                            // Direccion que empuja hacia arriba/afuera del pad
                            Vector3 pushDirection = (b.transform.position - player.position).normalized;
                            if (pushDirection.y < 0.2f) pushDirection.y = 0.8f; // Asegura impulso vertical
                            
                            ballRb.linearVelocity = pushDirection.normalized * ballRb.linearVelocity.magnitude;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    //  DESVENTAJA: PERDER UNA VIDA 
    void TakeDamage(GameObject playerObj)
    {
        if (GameManager.Instance != null)
        {
            // Resta vida según cuál sea el jugador
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