using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 10f;
    public float minY = 7f;
    public float maxY = 18f;
    public Rigidbody rb;
    public float input;

    [Header("Controles del Jugador")]
    public KeyCode keyUp;
    public KeyCode keyDown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FixedUpdate();       
    }

    void Awake(){
    	rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
    	Vector3 newPosition = rb.position + transform.right * input * speed * Time.fixedDeltaTime;

        newPosition.y = Mathf.Clamp(newPosition.y,minY,maxY);

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
