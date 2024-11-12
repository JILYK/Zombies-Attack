using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Префаб зомби

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

    IEnumerator AdjustDifficultyOverTime()
    {
        while (currentSpawnInterval > minimumSpawnInterval)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            currentSpawnInterval = Mathf.Max(minimumSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
       //     Debug.Log($"Увеличение сложности: новый интервал спавна {currentSpawnInterval} секунд.");
        }
    }

    IEnumerator SpawnZombies()
    {
        while (true)
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

            // Выбираем тип зомби на основе вероятности
            ZombieType chosenType = ChooseZombieType();
            if (chosenType != null)
            {
                // Создаем зомби внутри родительского объекта
                GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity, this.transform);
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

                // Отладочное сообщение в консоли
                //                Debug.Log($"Зомби типа {chosenType.name} спавнится в позиции {spawnPosition} со скоростью {chosenType.speed} и здоровьем {chosenType.health}");
            }
            else
            {
            //    Debug.LogWarning("Не удалось выбрать тип зомби для спавна.");
            }

            // Ожидание перед следующим спавном
            yield return new WaitForSeconds(currentSpawnInterval);
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
