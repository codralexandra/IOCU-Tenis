using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.position = initialPos;   
        }
    }
}
