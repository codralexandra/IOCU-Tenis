using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;
    int playerScore;
    int botScore;
    [SerializeField] public TextMeshProUGUI playerScoreText;
    [SerializeField] public TextMeshProUGUI botScoreText;
    [SerializeField] public ParticleSystem particleSystem1;
    [SerializeField] public ParticleSystem particleSystem2;
    [SerializeField] public string currentServer = "player"; // "player" or "bot"
    private TrailRenderer trailRenderer; // Reference to the trail renderer
    public bool playing = true;

    void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void TriggerConfettiParticles()
    {
        particleSystem1.Play();
        particleSystem2.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            ResetBall();
            if (playing)
            {
                if (hitter == "player")
                    playerScore++;
                else if (hitter == "bot")
                    botScore++;
                playing = false;
                UpdateScores();
            }
        }
        else if (collision.transform.CompareTag("Net"))
        {
            ResetBall();
            if (playing)
            {
                if (hitter == "player")
                    botScore++;
                else if (hitter == "bot")
                    playerScore++;
                playing = false;
                TriggerConfettiParticles();
                UpdateScores();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") && playing)
        {
            if (hitter == "player")
                playerScore++;
            else if (hitter == "bot")
                botScore++;
            playing = false;
            TriggerConfettiParticles();
            UpdateScores();
        }
    }

    private void UpdateScores()
    {
        playerScoreText.text = playerScore.ToString();
        botScoreText.text = botScore.ToString();
        currentServer = hitter;
    }

    private void ResetBall()
    {
        // Disable trail before resetting position
        
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject.Find("Player").GetComponent<PlayerController>().ResetForServe();
        GameObject.Find("Bot").GetComponent<Bot>().ResetForServe();

        
    }
    public void PositionForServe(Vector3 serverPosition, bool isBot)
    {
        trailRenderer.enabled = false;
        trailRenderer.Clear();

        // Different offset for bot vs player
        Vector3 offset = isBot ? new Vector3(-0.5f, 1, 0) : new Vector3(0.2f, 1, 0);
        transform.position = serverPosition + offset;

        // Re-enable trail after positioning
        trailRenderer.enabled = true;
    }

}