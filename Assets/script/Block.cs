using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject capsule;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ball"))
        {
            int posibility = Random.Range(0,10);
            if(posibility == 5)
            {
                //Instantiate(capsule,this.transform.position,capsule.transform.rotation);
                GameObject capLeft = Instantiate(capsule, transform.position, Quaternion.identity);
                Capsule capScriptLeft = capLeft.GetComponent<Capsule>();
                if (capScriptLeft != null)
                {
                    capScriptLeft.moveDirection = Vector3.left; // Viaja a la izquierda (P1)
                }

                GameObject capRight = Instantiate(capsule, transform.position, Quaternion.identity);
                Capsule capScriptRight = capRight.GetComponent<Capsule>();
                if (capScriptRight != null)
                {
                    capScriptRight.moveDirection = Vector3.right; // Viaja a la derecha (P2)
                }
            }
            GameManager.Instance.BlockDestroy();
            Destroy(this.gameObject);
        }
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Color randomColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
        GetComponent<Renderer>().material.color=randomColor;
    
    }
}
