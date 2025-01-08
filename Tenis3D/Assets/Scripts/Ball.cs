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
    private TrailRenderer trailRenderer; 
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
        Time.timeScale = 1f;

    }

    public void TriggerConfettiParticles()
    {
        particleSystem1.Play();
        particleSystem2.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Wall hits
        if (collision.transform.CompareTag("Wall"))
        {
            Debug.Log("Ball hit wall. Await reset");
            ResetBall();
            if (playing)
            {
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
                UpdateScores();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTerrain"))
        {
            hitPlayerTerrain = true;
            Debug.Log("Ball hit player terrain");
        }
        else if (other.CompareTag("BotTerrain"))
        {
            hitBotTerrain = true;
            Debug.Log("Ball hit bot terrain");
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
            Time.timeScale = 0f;
        }
    }

    private void ResetBall()
    {
  

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

        Vector3 offset = isBot ? new Vector3(-0.5f, 1, 0) : new Vector3(0.2f, 1, 0);
        transform.position = serverPosition + offset;


        trailRenderer.enabled = true;
    }

}