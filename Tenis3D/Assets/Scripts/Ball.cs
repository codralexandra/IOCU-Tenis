using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;

    int playerScore;
    int botScore;

    [SerializeField] public TextMeshProUGUI playerScoreText;
    [SerializeField] public TextMeshProUGUI botScoreText;

    public bool playing = true;

    void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            GameObject.Find("Player").GetComponent<PlayerController>().Reset();

            if (playing)
            {
                if (hitter == "player")
                    playerScore++;
                else if (hitter == "bot")
                    botScore++;
                playing = false;
            }
        }
        else if (collision.transform.CompareTag("Out"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            GameObject.Find("Player").GetComponent<PlayerController>().Reset();

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") && playing)
        {
            if (hitter == "player")
                botScore++;
            else if (hitter == "bot")
                playerScore++;
            playing = false;
            UpdateScores();
        }
    }

    private void UpdateScores()
    {
        playerScoreText.text = playerScore.ToString();
        botScoreText.text = botScore.ToString();
        transform.position = initialPos;
    }
}
