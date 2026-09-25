using UnityEngine;

public class SoloPlayer : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speed = 15f;
    public float minX = -4.5f; // Cambiado a X (límite izquierdo)
    public float maxX = 4.5f;  // Cambiado a X (límite derecho)

    [Header("Controles del Jugador")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyRight = KeyCode.D;

    [Header("Referencia al Pad Gemelo (Pad 2)")]
    public Transform otherPlayerPad;

    private Rigidbody rb;
    private Collider padCollider;
    private float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        padCollider = GetComponent<Collider>();
    }

    void Update()
    {
        input = 0f;

        // Teclas en A / D o Flechas
        if (Input.GetKey(keyLeft) || Input.GetKey(KeyCode.LeftArrow))
        {
            input = -1f;
        }
        else if (Input.GetKey(keyRight) || Input.GetKey(KeyCode.RightArrow))
        {
            input = 1f;
        }

        // Sincroniza la posición X del Pad 2 instantáneamente
        if (otherPlayerPad != null)
        {
            Vector3 otherPos = otherPlayerPad.position;
            otherPos.x = transform.position.x;
            otherPlayerPad.position = otherPos;
        }
    }

    void FixedUpdate()
    {
        // Mover horizontalmente en el eje X
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;

        float halfWidth = (padCollider != null) ? padCollider.bounds.extents.x : 0.5f;

        float clampedMin = minX + halfWidth;
        float clampedMax = maxX - halfWidth;

        // Limitar entre minX y maxX
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }
}