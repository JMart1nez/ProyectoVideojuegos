using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 10f;
    public float minY = 7f;
    public float maxY = 18f;
    public Rigidbody rb;
    public float input;
    private float leftWall;
    private float rightWall;
    private Collider padCollider;

    [Header("Controles del Jugador")]
    public KeyCode keyUp;
    public KeyCode keyDown;

    [Header("Referencia Multijugador / Solo")]
    public bool isPlayer1 = true;
    public player otherPlayerPad; // Asignar Pad 2 en Pad 1 y viceversa desde el Inspector

    void Awake()
    {        
        rb = GetComponent<Rigidbody>();
        padCollider = GetComponent<Collider>();
        float posWall = padCollider != null ? padCollider.bounds.extents.y : 0f;
        leftWall = minY - posWall;
        rightWall = maxY + posWall;
    }

    void Update()
    {
        input = 0f;

        // Si estamos en Solo y somos P2, no leemos teclado directo (P1 nos moverá)
        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo && !isPlayer1)
        {
            return;
        }

        // Lectura habitual de controles
        if (Input.GetKey(keyUp))
        {
            input = -1f;
        }
        else if (Input.GetKey(keyDown))
        {
            input = 1f;
        }

        // Si estamos en Solo y somos P1, replicamos nuestra dirección en P2
        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.Solo && isPlayer1 && otherPlayerPad != null)
        {
            otherPlayerPad.input = this.input;
        }
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + transform.right * input * speed * Time.fixedDeltaTime;

        float posWall = padCollider != null ? padCollider.bounds.extents.y : 0f;
        minY = leftWall + posWall;
        maxY = rightWall - posWall;

        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(newPosition);
    }
}