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
                Instantiate(capsule,this.transform.position,capsule.transform.rotation);
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
