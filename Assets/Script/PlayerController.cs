using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint; // Точка стрельбы
    public GameObject gunObject; // Объект пистолета
    public GameObject bulletPoolParent; // Родительский объект для всех пуль

    [Header("Настроки стрельбы - Shooting settings")]
    public float fireRate = 0.1f; // Скорострельность — 10 выстрелов в секунду

    public float bulletSpeed = 10f; // Скорость пули
    public int poolSize = 20; // Размер пула
    private Vector2 targetDirection = Vector2.right; // Изначально направление вправо

    private List<GameObject> bulletPool; // Пул для хранения пуль
    private int poolIndex = 0; // Индекс для отслеживания следующей пули
    private bool isFlipped = false; // Состояние отражения игрока

    private const float FlipThresholdHigh = 100f; // Угол для переключения в отраженное состояние
    private const float FlipThresholdLow = 80f; // Угол для возврата в обычное состояние

    void Start()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPoolParent.transform);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }


        InvokeRepeating(nameof(Shoot), 0f, fireRate);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetDirection = (touchPosition - (Vector2)firePoint.position).normalized;


            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            gunObject.transform.rotation = Quaternion.Euler(0, 0, angle);


            if (!isFlipped && (angle > FlipThresholdHigh || angle < -FlipThresholdHigh))
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
                gunObject.transform.localScale = new Vector3(-1, -1, gunObject.transform.localScale.z);
                isFlipped = true;
            }
            else if (isFlipped && (angle < FlipThresholdLow && angle > -FlipThresholdLow))
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
                gunObject.transform.localScale = new Vector3(1, 1, gunObject.transform.localScale.z);
                isFlipped = false;
            }
        }
    }

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

    private GameObject GetBulletFromPool()
    {
        GameObject bullet = bulletPool[poolIndex];
        poolIndex = (poolIndex + 1) % poolSize;
        return bullet;
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}