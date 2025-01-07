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

    public bool isGameOn;

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
        isGameOn = true;
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topSpin;
    }

    private void Update()
    {
        if (isGameOn)
        {
            HandleMovement();
            HandleHitting();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (isHitting)
        {
            // Allow both horizontal and vertical movement of aim target while hitting
            Vector3 aimMovement = new Vector3(horizontalInput, 0, verticalInput) * 2 * Speed * Time.deltaTime;
            aimTarget.Translate(aimMovement);
        }
        else if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * Speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    private void HandleHitting()
    {
        Ball ballScript = ball.GetComponent<Ball>();

        // Regular hitting controls (E and F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isHitting = true;
            currentShot = shotManager.topSpin;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isHitting = false;
            if (ballScript.playing)
            {
                ExecuteHit();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isHitting = true;
            currentShot = shotManager.flat;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isHitting = false;
            if (ballScript.playing)
            {
                ExecuteHit();
            }
        }

        // Serving controls (R and T)
        if (ballScript.currentServer == "player")
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T))
            {
                isHitting = true;
                currentShot = Input.GetKeyDown(KeyCode.R) ? shotManager.flatServe : shotManager.kickServe;
                GetComponent<BoxCollider>().enabled = false;
                animator.Play("serve-prepare");
            }

            if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
            {
                isHitting = false;
                GetComponent<BoxCollider>().enabled = true;
                ExecuteServe();
            }
        }
    }


    private void ExecuteHit()
    {
        Vector3 hitDirection = aimTarget.position - transform.position;
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        Ball ballScript = ball.GetComponent<Ball>();

        
        ballScript.hitPlayerTerrain = false;
        ballScript.hitBotTerrain = false;
        ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
        PlayHitAnimation();
        audioManager.PlaySFX(audioManager.ballHit);
        ball.GetComponent<Ball>().hitter = "player";
        Debug.Log("Player hit ball");
    }

    private void ExecuteServe()
    {
        // Use the new centralized method
        ball.GetComponent<Ball>().PositionForServe(transform.position, false);

        ResetForServe();
        Vector3 hitDirection = aimTarget.position - transform.position;
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

        ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
        animator.Play("serve");
        audioManager.PlaySFX(audioManager.ballHit);

        ball.GetComponent<Ball>().hitter = "player";
        ball.GetComponent<Ball>().playing = true;
        Debug.Log("Player served");
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
        Ball ballScript = ball.GetComponent<Ball>();

        ballScript.hitPlayerTerrain = false;
        ballScript.hitBotTerrain = false;
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
        }
        else
        {
            animator.Play("backhand");
        }
    }

    public void ResetForServe()
    {
        if (ball.GetComponent<Ball>().currentServer == "player")
        {
            transform.position = servedRight ? ServeRight.position : ServeLeft.position;
            servedRight = !servedRight;
            Debug.Log("Player position reset for serve");
        }
    }
}
