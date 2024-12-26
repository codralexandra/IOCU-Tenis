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

    public Transform[] targets;

    ShotManager shotManager;
    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
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
    
    Vector3 PickTarget()
    {
        int randomValue = UnityEngine.Random.Range(0, targets.Length-1);
        return targets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = UnityEngine.Random.Range(0, 2);
        if (randomValue == 0)
            return shotManager.topSpin;
        else return shotManager.flat;
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
        Shot currentShot = PickShot();

        Vector3 hitDirection = PickTarget() - transform.position;
        Rigidbody ballRigidbody = ballCollider.GetComponent<Rigidbody>();

        ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

        PlayHitAnimation();

        ball.GetComponent<Ball>().hitter = "bot";

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
