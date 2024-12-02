using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform ball;

    private const float Speed = 3f;
    private const float Force = 13f;

    private bool isHitting;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
            aimTarget.Translate(new Vector3(horizontalInput, 0, 0) * Speed * Time.deltaTime);
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
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isHitting = false;
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

        ballRigidbody.linearVelocity = hitDirection.normalized * Force + new Vector3(0, 6, 0);

        PlayHitAnimation();
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
}
