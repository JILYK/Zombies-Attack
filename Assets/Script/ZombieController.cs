using UnityEngine;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;
    public int scoreValue; // Очки за убийство
    public Slider healthBar; // Ссылка на слайдер здоровья

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private float currentSpeed; // Текущая скорость зомби для отображения в инспекторе
   // private Vector3 previousPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        //previousPosition = transform.position;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    public bool log;

    void FixedUpdate()
    {
        if (player != null && health > 0)
        {
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
            if (log)
            {
                currentSpeed = rb.velocity.magnitude;
                print("Коэффициент скорости: " + currentSpeed);
                /* if (Mathf.Approximately(currentSpeed, speed))
                {
                    print("Скорость соответствует одному юниту в секунду, определенному переменной speed.");
                }
                else
                {
                    print("Скорость не соответствует одному юниту в секунду, определенному переменной speed.");
                }
                print(transform.position - previousPosition + "!!!!!!");
            */
            }

     //       previousPosition = transform.position;
        }
    }
    /*  void Update()
    {
        if (player != null && health > 0)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
            if(log)
            {
                float currentSpeed = Vector2.Distance(transform.position, previousPosition) / Time.deltaTime;
                if (Mathf.Approximately(currentSpeed, speed))
                {
                    print("Скорость соответствует одному юниту в секунду, определенному переменной speed.");
                }
                else
                {
                    print("Скорость не соответствует одному юниту в секунду, определенному переменной speed.");
                }
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
            healthBar.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.AddScore(scoreValue);
        animator.SetTrigger("DieZomba");
    }

    // Этот метод будет вызван как Animation Event в конце анимации смерти
    public void DisableZombie()
    {
        Destroy(gameObject);
    }
}