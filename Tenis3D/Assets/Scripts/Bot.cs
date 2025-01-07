using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    AudioManager audioManager;
    float speed = 50f;
    Animator animator;
    public Transform ball;
    public Transform aimTarget;
    Vector3 targetPosition;
    private const float Force = 8.5f;
    public Transform[] targets;
    ShotManager shotManager;
    private bool isServing = false;
    private bool waitingToServe = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
    }

    void Update()
    {
        Move();
        CheckForServe();
    }

    void Move()
    {
        if (!isServing)
        {
            targetPosition.z = ball.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            Debug.Log("Bot moving to targetPosition");
        }
    }

    private void CheckForServe()
    {
        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript.currentServer == "bot" && !ballScript.playing && !waitingToServe && !isServing)
        {
            waitingToServe = true;
            // Start serve sequence after 2 seconds
            Invoke("HandleServe", 2f);
            Debug.Log("Bot is waiting to serve");
        }
    }
    public void HandleServe()
    {
        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript.currentServer == "bot" && !ballScript.playing)
        {
            StartServe();
            // Add small delay before executing serve
            Invoke("ExecuteServe", 1.0f);
            Debug.Log("Bot is serving");
        }
    }

    private void StartServe()
    {
        ResetForServe();
        isServing = true;
        animator.Play("serve-prepare");
    }

    private void ExecuteServe()
    {
        Shot serveShot = (UnityEngine.Random.value > 0.5f) ? shotManager.flatServe : shotManager.kickServe;

        // Use the new centralized method
        ball.GetComponent<Ball>().PositionForServe(transform.position, true);

        Vector3 serveTarget = PickTarget();
        Vector3 hitDirection = serveTarget - transform.position;

        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.linearVelocity = hitDirection.normalized * serveShot.hitForce + new Vector3(0, serveShot.upForce, 0);

        animator.Play("serve");
        audioManager.PlaySFX(audioManager.ballHit);

        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.hitter = "bot";
        ballScript.playing = true;

        isServing = false;
        Debug.Log("Bot served");
    }
    Vector3 PickTarget()
    {
        int randomValue = UnityEngine.Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = UnityEngine.Random.Range(0, 2);
        if (randomValue == 0)
            return shotManager.topSpin;
        else
            return shotManager.flat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !isServing)
        {
            HitBall(other);
        }
    }

    private void HitBall(Collider ballCollider)
    {
        Ball ballScript = ball.GetComponent<Ball>();

        // Reset terrain hits
        ballScript.hitPlayerTerrain = false;
        ballScript.hitBotTerrain = false;

        Shot currentShot = PickShot();
        Vector3 hitDirection = PickTarget() - transform.position;
        Rigidbody ballRigidbody = ballCollider.GetComponent<Rigidbody>();
        ballRigidbody.linearVelocity = hitDirection.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
        PlayHitAnimation();
        audioManager.PlaySFX(audioManager.ballHit);
        ball.GetComponent<Ball>().hitter = "bot";
        Debug.Log("Bot hit the ball");
    }

    private void PlayHitAnimation()
    {
        Vector3 ballDirection = ball.position - transform.position;
        if (ballDirection.z >= 0)
        {
            animator.Play("backhand");
        }
        else
        {
            animator.Play("forehand");
        }
    }

    public void ResetForServe()
    {
        if (ball.GetComponent<Ball>().currentServer == "bot")
        {
            transform.position = new Vector3(6.91f, 0.717f, -0.39f);
            isServing = false;
            waitingToServe = false;
            Debug.Log("Bot position reset for serve.");
        }
        
    }
}