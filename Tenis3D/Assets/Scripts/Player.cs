using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform ball;

    private const float Speed = 3f;
    private const float Force = 8.5f;
    Vector3 aimTargetInitialPosition;

    private bool isHitting;
    private Animator animator;
    ShotManager shotManager;
    Shot currentShot;

    [SerializeField] Transform ServeRight;
    [SerializeField] Transform ServeLeft;

    bool servedRight = true;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topSpin;
    }

    private void Update()
    {
        HandleMovement();
        HandleHitting();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (isHitting)
        {
            aimTarget.Translate(new Vector3(horizontalInput, 0, 0) * 2 * Speed * Time.deltaTime);
        }
        else if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * Speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    private void HandleHitting()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isHitting = true;
            currentShot = shotManager.topSpin;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isHitting = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isHitting = true;
            currentShot = shotManager.flat;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isHitting = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isHitting = true;
            currentShot = shotManager.flatServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isHitting = true;
            currentShot = shotManager.kickServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");

        }
        else if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
        {
            isHitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);

            //luate din HitBall function 
            Vector3 hitDirection = aimTarget.position - transform.position;
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

            ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            animator.Play("serve");
            audioManager.PlaySFX(audioManager.ballHit);

            ball.GetComponent<Ball>().hitter = "player";
            ball.GetComponent<Ball>().playing = true;
        }

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

        ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

        PlayHitAnimation();
        audioManager.PlaySFX(audioManager.ballHit);

        ball.GetComponent<Ball>().hitter = "player";
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

    public void Reset()
    {
        if (servedRight)
            transform.position = ServeLeft.position;
        else
            transform.position = ServeRight.position;
        servedRight = !servedRight;
    }
}
