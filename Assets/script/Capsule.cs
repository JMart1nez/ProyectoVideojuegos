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
        
        // 0 y 1: Poderes | 2 y 3: Desventajas
        type = Random.Range(0, 4); 
        // 1: Limites de tamanio, ajustarlo para que quede dentro
        // 3: Limites de tamanio, ajustar limites de movimiento para que no haya espacios
    }

    void Update()
    {
        // Movimiento hacia abajo
        //transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

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
                    StartCoroutine(ScalePlayer(other.transform, 1.5f, 5f));
                    break;
                case 2: // Desventaja: Pelota veloz
                    StartCoroutine(ExtraSpeed());
                    break;
                case 3: // Desventaja: Reducir jugador
                    StartCoroutine(ScalePlayer(other.transform, 0.6f, 5f));
                    break;
            }

            Destroy(gameObject, 6f);
        }
    }

    void MultiBall()
    {
        if (currentBall == null) return;
        
        for (int i = 0; i < 2; i++)
        {
            GameObject newBall = Instantiate(prefabBall, currentBall.position, Quaternion.identity);
            Ball ballScript = newBall.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.Launch();
            }
        }
    }

    IEnumerator ScalePlayer(Transform player, float factor, float duration)
    {
        Vector3 originalScale = player.localScale;
        player.localScale = new Vector3(originalScale.x * factor, originalScale.y, originalScale.z);
        yield return new WaitForSeconds(duration);
        player.localScale = originalScale;
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
}