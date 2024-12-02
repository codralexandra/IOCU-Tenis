using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    void Start()
    {
        initialPos = transform.position;
    }

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
