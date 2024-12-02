using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Transform aimTarget; 

    float speed = 3f;
    float force = 13;

    bool hitting;

    [SerializeField] Transform ball;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
            hitting = true;
        else if (Input.GetKeyUp(KeyCode.F))
            hitting = false;

        if (hitting)
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * Time.deltaTime);
        }

        if ( (h != 0 || v != 0) && !hitting )
        {
            transform.Translate( new Vector3 (h, 0, v) * speed * Time.deltaTime );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 direction=aimTarget.position-transform.position;
            other.GetComponent<Rigidbody>().linearVelocity = direction.normalized * force +new Vector3(0,6,0);
            //Detect the direction of hit to play the correct animation
            Vector3 ballDir = ball.position - transform.position;
            if(ballDir.z >= 0)
                animator.Play("forehand");
            else
                animator.Play("backhand");
        }
    }
}
