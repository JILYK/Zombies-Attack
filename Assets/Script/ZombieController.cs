using UnityEngine;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    public float speed;               // Постоянная скорость зомби
    public int health;                // Начальное здоровье зомби
    public int maxHealth;             // Максимальное здоровье зомби
    public int scoreValue;            // Очки за убийство
    public Slider healthBar;          // Ссылка на слайдер здоровья

    private Transform player;         // Ссылка на игрока
    private Animator animator;        // Ссылка на аниматор

    [SerializeField]
    private float currentSpeed;       // Текущая скорость зомби для отображения в инспекторе

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Находим игрока по тегу
        animator = GetComponent<Animator>(); // Получаем компонент Animator

        // Инициализируем здоровье зомби
        health = maxHealth;

        // Устанавливаем maxValue для слайдера здоровья
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health; // Устанавливаем текущее значение здоровья на слайдере
        }
    }

    void Update()
    {
        // Движение зомби к игроку с постоянной скоростью, если зомби жив
        if (player != null && health > 0)
        {
            Vector2 direction = (player.position - transform.position).normalized; // Нормализуем направление к игроку
            transform.position += (Vector3)(direction * speed * Time.deltaTime);   // Движение с постоянной скоростью

            // Обновляем значение текущей скорости для инспектора
            currentSpeed = speed; // Скорость должна быть постоянной, равной переменной speed
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthBar != null)
        {
            healthBar.value = health; // Обновляем значение слайдера при получении урона
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.AddScore(scoreValue); // Добавить очки
        animator.SetTrigger("DieZomba"); // Запускаем анимацию смерти
    }

    // Этот метод будет вызван как Animation Event в конце анимации смерти
    public void DisableZombie()
    {
        Destroy(gameObject);
    }
}
