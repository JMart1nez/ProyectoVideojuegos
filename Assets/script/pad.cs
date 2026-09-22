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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FixedUpdate();       
    }

    void Awake()
    {        
    	rb = GetComponent<Rigidbody>();
        padCollider = GetComponent<Collider>();
        // Calculamos la posicion real de las paredes sumando el tamanio inicial
        float posWall = padCollider != null ? padCollider.bounds.extents.y : 0f;
        leftWall = minY - posWall;
        rightWall = maxY + posWall;
    }
    
    void FixedUpdate()
    {
    	Vector3 newPosition = rb.position + transform.right * input * speed * Time.fixedDeltaTime;

        // Calculamos la posicion real de las paredes (por si cambio de tamanio con un Power-Up)
        float posWall = padCollider != null ? padCollider.bounds.extents.y : 0f;

        // Ajustamos los limites exactos sin dejar espacio fantasma
        minY = leftWall + posWall;
        maxY = rightWall - posWall;

        // Limitar la posicion dentro del nuevo rango dinamico
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(newPosition);
    }

    // Update is called once per frame
    void Update()
    {
        input = 0f;

        if (Input.GetKey(keyUp))
        {
            input = -1f;
        }
        else if (Input.GetKey(keyDown))
        {
            input = 1f;
        }
    }
}
