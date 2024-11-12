using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;       // Префаб пули
    public Transform firePoint;           // Точка стрельбы
    public GameObject gunObject;          // Объект пистолета
    public GameObject bulletPoolParent;   // Родительский объект для всех пуль
    public float fireRate = 0.1f;         // Скорострельность — 10 выстрелов в секунду
    public float bulletSpeed = 10f;       // Скорость пули
    public int poolSize = 20;             // Размер пула
    private Vector2 targetDirection = Vector2.right; // Изначально направление вправо

    private List<GameObject> bulletPool;  // Пул для хранения пуль
    private int poolIndex = 0;            // Индекс для отслеживания следующей пули
    private bool isFlipped = false;       // Состояние отражения игрока

    private const float FlipThresholdHigh = 100f;    // Угол для переключения в отраженное состояние
    private const float FlipThresholdLow = 80f;      // Угол для возврата в обычное состояние

    void Start()
    {
        // Создаем пул пуль как дочерние объекты bulletPoolParent
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPoolParent.transform); // Создаем пулю как дочерний объект bulletPoolParent
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }

        // Запускаем постоянную стрельбу
        InvokeRepeating(nameof(Shoot), 0f, fireRate);
    }

    void Update()
    {
        // Обновление направления и поворот пистолета при удержании пальца на экране
        if (Input.GetMouseButton(0)) // Удержание пальца
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetDirection = (touchPosition - (Vector2)firePoint.position).normalized;

            // Поворот пистолета к точке тапа
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            gunObject.transform.rotation = Quaternion.Euler(0, 0, angle); // Пистолет смотрит на тап

            // Проверка на изменение отражения игрока
            if (!isFlipped && (angle > FlipThresholdHigh || angle < -FlipThresholdHigh))
            {
                // Отражаем игрока и пистолет по X и Y
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                gunObject.transform.localScale = new Vector3(-1, -1, gunObject.transform.localScale.z);
                isFlipped = true;
            }
            else if (isFlipped && (angle < FlipThresholdLow && angle > -FlipThresholdLow))
            {
                // Возвращаем игрока и пистолет к обычному масштабу
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                gunObject.transform.localScale = new Vector3(1, 1, gunObject.transform.localScale.z);
                isFlipped = false;
            }
        }
    }

    // Метод стрельбы с использованием Object Pooling
    void Shoot()
    {
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(false);
            bullet.SetActive(true);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = targetDirection * bulletSpeed;
            }
        }
    }

    // Метод для получения пули из пула
    private GameObject GetBulletFromPool()
    {
        GameObject bullet = bulletPool[poolIndex];
        poolIndex = (poolIndex + 1) % poolSize; // Переход к следующему элементу в пуле
        return bullet;
    }

    // Метод для возвращения пули в пул
    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
