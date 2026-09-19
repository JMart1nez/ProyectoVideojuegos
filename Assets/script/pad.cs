using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 10f;
    public float minX = -6f;
    public float maxX = 6f;
    public Rigidbody rb;
    public float input;
    
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
    	Vector3 newPosition = rb.position + Vector3.right * input * speed
    				* Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x,minX,maxX);

        rb.MovePosition(newPosition);
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxis("Horizontal");
    }
}
