using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;
    public string winner;
    int playerScore;
    int botScore;
    [SerializeField] public TextMeshProUGUI playerScoreText;
    [SerializeField] public TextMeshProUGUI botScoreText;
    [SerializeField] public ParticleSystem particleSystem1;
    [SerializeField] public ParticleSystem particleSystem2;
    [SerializeField] public string currentServer = "player"; // "player" or "bot"
    private TrailRenderer trailRenderer; // Reference to the trail renderer
    public bool playing = true;

    public bool hitPlayerTerrain = false;
    public bool hitBotTerrain = false;
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
        // Check for terrain hits first
        if (collision.transform.CompareTag("PlayerTerrain"))
        {
            hitPlayerTerrain = true;
        }
        else if (collision.transform.CompareTag("BotTerrain"))
        {
            hitBotTerrain = true;
        }
        // Wall hits
        else if (collision.transform.CompareTag("Wall"))
        {
            Debug.Log("wall");
            ResetBall();
            if (playing)
            {
                // Award point to hitter if ball hit the correct terrain first
                if (hitter == "player" && hitBotTerrain)
                {
                    playerScore++;
                    winner = hitter;
                    
                }
                else if (hitter == "bot" && hitPlayerTerrain)
                {
                    botScore++;
                    winner = hitter;
                }
                // If ball didn't hit correct terrain, point goes to opponent
                else if (hitter == "player" && !hitBotTerrain)
                {
                    botScore++;
                    winner = "bot";
                }
                else if (hitter == "bot" && !hitPlayerTerrain)
                {
                    playerScore++;
                    winner = "player";
                }
                playing = false;
                UpdateScores();
            }
        }
        else if (collision.transform.CompareTag("Net"))
        {
            Debug.Log("Net");
            ResetBall();
            if (playing)
            {
                if (hitter == "player")
                {
                    botScore++;
                    winner = "bot";
                }
                else if (hitter == "bot")
                {
                    playerScore++;
                    winner = "player";
                }
                playing = false;
                TriggerConfettiParticles();
                UpdateScores();
            }
        }
        Debug.Log(winner);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") && playing)
        {
            Debug.Log("Out");
            // Award point to hitter if ball hit the correct terrain first
            if (hitter == "player" && hitBotTerrain)
            {
                playerScore++;
                winner = hitter;
            }
            else if (hitter == "bot" && hitPlayerTerrain)
            {
                botScore++;
                winner = hitter;
            }
            // If ball didn't hit correct terrain, point goes to opponent
            else if (hitter == "player" && !hitBotTerrain)
            {
                botScore++;
                winner = "bot";
            }
            else if (hitter == "bot" && !hitPlayerTerrain)
            {
                playerScore++;
                winner = "player";
            }
            hitBotTerrain = false;
            hitPlayerTerrain = false;
            playing = false;
            TriggerConfettiParticles();
            UpdateScores();
            Debug.Log(winner);
        }
    }
    private void UpdateScores()
    {
        playerScoreText.text = playerScore.ToString();
        botScoreText.text = botScore.ToString();
        currentServer = winner;
        Debug.Log("Point to " + winner.ToString());
    }

    private void ResetBall()
    {
        // Disable trail before resetting position
        Debug.Log("Ball reset");

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