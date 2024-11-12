using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController playerController; // Ссылка на контроллер игрока для возврата пули в пул

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, сталкивается ли пуля с зомби
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.TakeDamage(1); // Уменьшаем здоровье зомби на 1

            // Отладка: выводим информацию о попадании и новом уровне здоровья зомби
        //    Debug.Log($"Пуля попала в зомби {zombie.gameObject.name}. Здоровье зомби: {zombie.health}");

            playerController.ReturnBulletToPool(gameObject); // Возвращаем пулю в пул
        }
        else
        {
            // Отладка: выводим сообщение, если пуля сталкивается с объектом, не являющимся зомби
         //   Debug.Log($"Пуля столкнулась с объектом {collision.gameObject.name}, который не является зомби.");
        }
    }
}