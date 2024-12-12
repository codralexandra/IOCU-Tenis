using System;
using UnityEngine;

public class Bot : MonoBehaviour
{

    float speed = 50f;
    Animator animator;
    public Transform ball;
    public Transform aimTarget;
    Vector3 targetPosition;
    private const float Force = 8.5f;


    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        targetPosition.z = ball.position.z;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            HitBall(other);
        }
    }

    private void HitBall(Collider ballCollider)
    {
        Vector3 hitDirection = aimTarget.position - transform.position;
        Rigidbody ballRigidbody = ballCollider.GetComponent<Rigidbody>();

        ballRigidbody.linearVelocity = hitDirection.normalized * Force + new Vector3(0, 5, 0);

        PlayHitAnimation();
    }

    private void PlayHitAnimation()
    {
        Vector3 ballDirection = ball.position - transform.position;

        if (ballDirection.z >= 0)
        {
            animator.Play("forehand");
            Console.WriteLine("Forehand");
        }
        else
        {
            animator.Play("backhand");
            Console.WriteLine("Backhand");
        }
    }
}
