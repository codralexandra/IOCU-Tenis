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

    public bool playing = true;

    void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
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
        //transform.position = initialPos;

        // Change server to the player who won the point
        currentServer = hitter;
    }

    private void ResetBall()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (currentServer == "player")
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ResetForServe();
            transform.position = GameObject.Find("Player").transform.position + new Vector3(0.2f, 1, 0); // Position ball near the player
        }
        else if (currentServer == "bot")
        {
            GameObject.Find("Bot").GetComponent<Bot>().ResetForServe();
            transform.position = new Vector3(6.91f, 1.0f, -0.39f); // Adjusted position for the bot serve
        }
    }

}
