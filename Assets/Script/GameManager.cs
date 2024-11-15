using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int Score
    {
        get { return score; }
    }

    public void AddScore(int points)
    {
        score += points;
//        Debug.Log("Score: " + score); // Отображение очков
    }
}