using System;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject EndGame; // Объект EndGame, который будет активироваться
    public Text endGameScoreText; // Текст, который будет отображать очки при окончании игры
    public Animator animator;
    
    private void Awake()
    {
        animator.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // Проверяем, сталкивается ли пуля с зомби
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            animator.enabled = true;
            print("!!!!!!" + collision.name);
            animator.SetTrigger("EndGame");

            if (endGameScoreText != null)
            {
                int score = GameManager.Instance.Score; // Получаем текущее количество очков
                int highScore = PlayerPrefs.GetInt("HighScore", 0); // Получаем сохраненный рекорд, по умолчанию 0

                // Проверяем, является ли текущий счет новым рекордом
                if (score > highScore)
                {
                    PlayerPrefs.SetInt("HighScore", score); // Сохраняем новый рекорд
                    PlayerPrefs.Save(); // Сохраняем изменения
                    endGameScoreText.GetComponent<TypewriterEffect>().fullText = $"Это пекорд: {score}!"; // Текст для нового рекорда
                }
                else
                {
                    endGameScoreText.GetComponent<TypewriterEffect>().fullText = $"Очки: {score} (Рекорд: {highScore})"; // Текст с обычными очками и рекордом
                }
            }

            EndGame.SetActive(true); // Активируем объект EndGame
            print("Game Over"); // Сообщение о конце игры
        }
    }
}