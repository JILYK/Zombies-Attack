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
    private Rigidbody2D rb;
    [SerializeField]
    private float currentSpeed;       // Текущая скорость зомби для отображения в инспекторе
    private Vector3 previousPosition; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Находим игрока по тегу
        animator = GetComponent<Animator>(); // Получаем компонент Animator
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
        // Инициализируем здоровье зомби
        health = maxHealth;
        previousPosition = transform.position;
        // Устанавливаем maxValue для слайдера здоровья
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health; // Устанавливаем текущее значение здоровья на слайдере
        }
    }

    public bool log;
    void FixedUpdate() // Use FixedUpdate for physics calculations
    {
        if (player != null && health > 0)
        {
            Vector2 direction = player.position - transform.position;
            direction.Normalize(); // Make direction a unit vector

            rb.velocity = direction * speed; // Set velocity directly

            if (log)
            {
                currentSpeed = rb.velocity.magnitude;
                print("Коэффициент скорости: " + currentSpeed);
                if (Mathf.Approximately(currentSpeed, speed))
                {
                    print("Скорость соответствует одному юниту в секунду, определенному переменной speed.");
                }
                else
                {
                    print("Скорость не соответствует одному юниту в секунду, определенному переменной speed.");
                }
                print(transform.position - previousPosition + "!!!!!!сука +");
            }
            previousPosition = transform.position;
        }
    }
    /*  void Update()
    {
        // Движение зомби к игроку с постоянной скоростью
        if (player != null && health > 0)
        {
            // Постоянное движение к игроку с использованием Vector2.MoveTowards
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
            if(log) 
            {
                float currentSpeed = Vector2.Distance(transform.position, previousPosition) / Time.deltaTime;
                print("Коэффициент скорости: " + currentSpeed);
                if (Mathf.Approximately(currentSpeed, speed))
                {
                    print("Скорость соответствует одному юниту в секунду, определенному переменной speed.");
                }
                else
                {
                    print("Скорость не соответствует одному юниту в секунду, определенному переменной speed.");
                }
                print(transform.position - previousPosition + "!!!!!!сука +");
            }
            previousPosition = transform.position;
        }
    }
*/

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
