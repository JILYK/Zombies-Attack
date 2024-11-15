using UnityEngine;

public class MenuUIManadger : MonoBehaviour
{
    public TypewriterEffect text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            Destroy(collision.gameObject);
        }
    }
    private void Awake()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        text.fullText = $"Твой рекорд: {highScore}"; 
    }
}