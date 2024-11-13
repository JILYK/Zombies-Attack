using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManadger : MonoBehaviour
{
    public TypewriterEffect text;
    // Этот метод вызывается при столкновении с другим объектом
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, сталкивается ли пуля с зомби
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            Destroy(collision.gameObject);
        }
    }

    private void Awake()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); // Получаем сохраненный рекорд, по умолчанию 0
        text.fullText = $"Твой рекорд: {highScore}"; 
    }
}