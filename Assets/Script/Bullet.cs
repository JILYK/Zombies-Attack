using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ZombieController zombie = collision.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.TakeDamage(1);
            playerController.ReturnBulletToPool(gameObject); // Возвращаем пулю в пул
        }
    }
}