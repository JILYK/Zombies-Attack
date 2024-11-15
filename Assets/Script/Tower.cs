using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject EndGame;
    public Text endGameScoreText;
    public Animator animator;

    private void Awake()
    {
        animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            animator.enabled = true;
            print("!!!!!!" + collision.name);
            animator.SetTrigger("EndGame");

            if (endGameScoreText != null)
            {
                int score = GameManager.Instance.Score;
                int highScore = PlayerPrefs.GetInt("HighScore", 0);


                if (score > highScore)
                {
                    PlayerPrefs.SetInt("HighScore", score);
                    PlayerPrefs.Save();
                    endGameScoreText.GetComponent<TypewriterEffect>().fullText = $"Это пекорд: {score}!";
                }
                else
                {
                    endGameScoreText.GetComponent<TypewriterEffect>().fullText = $"Очки: {score} (Рекорд: {highScore})";
                }
            }

            EndGame.SetActive(true);
        }
    }
}