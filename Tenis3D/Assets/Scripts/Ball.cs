using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;
    public string winner;
    int playerScore;
    int botScore;
    int maxScore = 5;
    [SerializeField] public TextMeshProUGUI playerScoreText;
    [SerializeField] public TextMeshProUGUI botScoreText;
    [SerializeField] public ParticleSystem particleSystem1;
    [SerializeField] public ParticleSystem particleSystem2;
    [SerializeField] public string currentServer = "player"; // "player" or "bot"
    private TrailRenderer trailRenderer; // Reference to the trail renderer
    public bool playing = true;

    public bool hitPlayerTerrain = false;
    public bool hitBotTerrain = false;

    public GameObject endMenuUI;
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
            Debug.Log("Ball hit player terrain");
        }
        else if (collision.transform.CompareTag("BotTerrain"))
        {
            hitBotTerrain = true;
            Debug.Log("Ball hit bot terrain");
        }
        // Wall hits
        else if (collision.transform.CompareTag("Wall"))
        {
            Debug.Log("Ball hit wall. Await reset");
            ResetBall();
            if (playing)
            {
                // Award point to hitter if ball hit the correct terrain first
                if (hitter == "player" && hitBotTerrain)
                {
                    playerScore++;
                    winner = hitter;
                    Debug.Log("Point to player. Bot missed shot.");
                }
                else if (hitter == "bot" && hitPlayerTerrain)
                {
                    botScore++;
                    winner = hitter;
                    Debug.Log("Point to bot. Player missed shot.");
                }
                // If ball didn't hit correct terrain, point goes to opponent
                else if (hitter == "player" && !hitBotTerrain)
                {
                    botScore++;
                    winner = "bot";
                    Debug.Log("Point to bot. Player hit out.");
                }
                else if (hitter == "bot" && !hitPlayerTerrain)
                {
                    playerScore++;
                    winner = "player";
                    Debug.Log("Point to player. Bot hit out.");
                }
                playing = false;
                UpdateScores();
            }
        }
        else if (collision.transform.CompareTag("Net"))
        {
            Debug.Log("Ball hit net");
            ResetBall();
            if (playing)
            {
                if (hitter == "player")
                {
                    botScore++;
                    winner = "bot";
                    Debug.Log("Point to bot. Player hit net.");
                }
                else if (hitter == "bot")
                {
                    playerScore++;
                    winner = "player";
                    Debug.Log("Point to player. Bot hit net.");
                }
                playing = false;
                //TriggerConfettiParticles();
                UpdateScores();
            }
        }
        else if (collision.transform.CompareTag("Out"))
        {
            Debug.Log("Ball hit outside the playing field.");
            // Award point to hitter if ball hit the correct terrain first
            if (hitter == "player" && hitBotTerrain)
            {
                playerScore++;
                winner = hitter;
                Debug.Log("Point to player. Bot didnt catch.");
            }
            else if (hitter == "bot" && hitPlayerTerrain)
            {
                botScore++;
                winner = hitter;
                Debug.Log("Point to bot. Player didnt catch.");
            }
            // If ball didn't hit correct terrain, point goes to opponent
            else if (hitter == "player" && !hitBotTerrain)
            {
                botScore++;
                winner = "bot";
                Debug.Log("Point to bot. Player hit out.");
            }
            else if (hitter == "bot" && !hitPlayerTerrain)
            {
                playerScore++;
                winner = "player";
                Debug.Log("Point to player. Bot hit out.");
            }
            hitBotTerrain = false;
            hitPlayerTerrain = false;
            playing = false;
            //TriggerConfettiParticles();
            UpdateScores();
        }
    }

    private void UpdateScores()
    {
        playerScoreText.text = playerScore.ToString();
        botScoreText.text = botScore.ToString();
        TriggerConfettiParticles();
        currentServer = winner;
        Debug.Log("Scores updated!");
        if(playerScore == maxScore || botScore == maxScore)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isGameOn = false;
            endMenuUI.SetActive(true);
            Debug.Log("Game ended");
        }
    }

    private void ResetBall()
    {
        // Disable trail before resetting position

        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject.Find("Player").GetComponent<PlayerController>().ResetForServe();
        GameObject.Find("Bot").GetComponent<Bot>().ResetForServe();

        Debug.Log("Ball position reset");
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