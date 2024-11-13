using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Префаб зомби
    public Transform poolZombies; // Ссылка на объект PoolZombi, внутри которого находятся зомби
    private List<Transform> zombieList = new List<Transform>();
    // Массив объектов зон спавна
    public GameObject[] spawnAreas = new GameObject[3];


    
    
    // Параметры для разных типов зомби
    [System.Serializable]
    public class ZombieType
    {
        public string name;
        public float speed;
        public int health;
        public int scoreValue;
        public float spawnChance; // Вероятность спавна
        public Color color;       // Цвет зомби
    }

    public ZombieType[] zombieTypes; // Массив типов зомби

    // Настройки сложности
    public float initialSpawnInterval = 2f; // Начальный интервал спавна
    public float difficultyIncreaseInterval = 10f; // Интервал увеличения сложности
    public float spawnIntervalDecrease = 0.1f; // Снижение интервала спавна
    public float minimumSpawnInterval = 0.5f; // Минимальное значение интервала спавна

    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval; // Устанавливаем начальный интервал
        StartCoroutine(AdjustDifficultyOverTime());
        StartCoroutine(SpawnZombies());
    }

    void FixedUpdate()
    {
        SortZombies(); // Сортируем зомби по оси Y каждый кадр
    }

    IEnumerator AdjustDifficultyOverTime()
    {
        while (currentSpawnInterval > minimumSpawnInterval)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            currentSpawnInterval = Mathf.Max(minimumSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
            Debug.Log($"Увеличение сложности: новый интервал спавна {currentSpawnInterval} секунд.");
        }
    }

    IEnumerator SpawnZombies()
    {
      
        while (true)
        {
            if (zombieList.Count < 120)
            {
                
            // Выбираем случайную область спавна
            GameObject chosenArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

            // Получаем размеры и положение из BoxCollider зоны спавна
            BoxCollider2D areaCollider = chosenArea.GetComponent<BoxCollider2D>();
            if (areaCollider == null)
            {
                Debug.LogWarning("BoxCollider2D не найден на объекте зоны спавна.");
                yield break;
            }

            Vector2 spawnPosition = new Vector2(
                Random.Range(areaCollider.bounds.min.x, areaCollider.bounds.max.x),
                Random.Range(areaCollider.bounds.min.y, areaCollider.bounds.max.y)
            );
            
            ZombieType chosenType = ChooseZombieType();
            if (chosenType != null)
            {
                // Создаем зомби внутри объекта poolZombies
                GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity, poolZombies);
                ZombieController zombieController = zombie.GetComponent<ZombieController>();

                // Устанавливаем параметры зомби
                zombieController.speed = chosenType.speed;
                zombieController.maxHealth = chosenType.health;
                zombieController.scoreValue = chosenType.scoreValue;

                // Устанавливаем цвет зомби
                Image zombieImage = zombie.GetComponent<Image>();
                if (zombieImage != null)
                {
                    zombieImage.color = chosenType.color;
                }
            zombieList.Add(zombie.transform);
            }
            // Ожидание перед следующим спавном
            yield return new WaitForSeconds(currentSpawnInterval);
            }
        }
    }

    ZombieType ChooseZombieType()
    {
        float randomValue = Random.value * 100;
        float cumulativeProbability = 0;

        foreach (var type in zombieTypes)
        {
            cumulativeProbability += type.spawnChance;
            if (randomValue <= cumulativeProbability)
            {
                return type;
            }
        }
        return zombieTypes[zombieTypes.Length - 1]; // Возвращаем последний тип по умолчанию, если выбор не удался
    }

    // Метод для сортировки зомби по оси Y
    void SortZombies()
    {
        // Удаляем уничтоженные объекты из списка
        zombieList.RemoveAll(zombie => zombie == null);

        // Пороговое значение для сравнения (эпсилон)
        float epsilon = 0.01f;  // Можно настроить в зависимости от нужной точности

        // Сортировка списка зомби по оси Y в обратном порядке с учётом эпсилона
        zombieList.Sort((img1, img2) =>
        {
            float difference = img1.transform.position.y - img2.transform.position.y;
            if (Mathf.Abs(difference) < epsilon)
            {
                return 0; // Считаем их равными, если разница меньше эпсилона
            }
            return difference > 0 ? -1 : 1; // Меняем местами для обратной сортировки
        });

        // Применение порядка отрисовки
        for (int i = 0; i < zombieList.Count; i++)
        {
            zombieList[i].transform.SetSiblingIndex(i);
        }
    }




    // Визуализация областей спавна в редакторе
    private void OnDrawGizmos()
    {
        Color[] colors = { Color.green, Color.blue, Color.red }; // Цвета для каждой области спавна

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            if (spawnAreas[i] != null)
            {
                BoxCollider2D areaCollider = spawnAreas[i].GetComponent<BoxCollider2D>();
                if (areaCollider != null)
                {
                    Gizmos.color = new Color(colors[i].r, colors[i].g, colors[i].b, 0.3f); // Полупрозрачный цвет для заливки
                    Gizmos.DrawCube(areaCollider.bounds.center, areaCollider.bounds.size);  // Заливка области

                    Gizmos.color = colors[i];
                    Gizmos.DrawWireCube(areaCollider.bounds.center, areaCollider.bounds.size); // Контур области
                }
            }
        }
    }
}
