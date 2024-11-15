using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform poolZombies;
    private List<Transform> zombieList = new List<Transform>();
    public GameObject[] spawnAreas = new GameObject[3];


    [System.Serializable]
    public class ZombieType
    {
        public string name;
        public float speed;
        public int health;
        public int scoreValue;
        public float spawnChance;
        public Color color;
    }

    public ZombieType[] zombieTypes;

    // Настройки сложности
    public float initialSpawnInterval = 2f; // Начальный интервал спавна
    public float difficultyIncreaseInterval = 10f; // Интервал увеличения сложности
    public float spawnIntervalDecrease = 0.1f; // Снижение интервала спавна
    public float minimumSpawnInterval = 0.5f; // Минимальное значение интервала спавна

    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
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
        int indexZombie = 0;
        while (true)
        {
            indexZombie++;
            if (zombieList.Count < 120)
            {
                GameObject chosenArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
                BoxCollider2D areaCollider = chosenArea.GetComponent<BoxCollider2D>();
                if (areaCollider == null)
                {
                    Debug.LogWarning("BoxCollider2D не найден на объекте зоны спавна.");
                    yield break;
                }

                // кеширование bounds
                Bounds bounds = areaCollider.bounds; // Сохраняем bounds в переменную
                Vector2 spawnPosition = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );

                ZombieType chosenType = ChooseZombieType();
                if (chosenType != null)
                {
                    GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity, poolZombies);
                    zombie.name += indexZombie;
                    RectTransform rectTransform = zombie.GetComponent<RectTransform>();
                    Vector3 newPosition = rectTransform.localPosition;
                    newPosition.z = 0f; // Обнуляем координату Z
                    rectTransform.localPosition = newPosition;

                    ZombieController zombieController = zombie.GetComponent<ZombieController>();
                    zombieController.speed = chosenType.speed;
                    zombieController.maxHealth = chosenType.health;
                    zombieController.scoreValue = chosenType.scoreValue;
                    Image zombieImage = zombie.GetComponent<Image>();
                    if (zombieImage != null)
                    {
                        zombieImage.color = chosenType.color;
                    }

                    zombieList.Add(zombie.transform);
                }

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

        return zombieTypes[zombieTypes.Length - 1];
    }

    void SortZombies()
    {
        zombieList.RemoveAll(zombie => zombie == null);
        float epsilon = 0.01f;
        zombieList.Sort((img1, img2) =>
        {
            float difference = img1.transform.position.y - img2.transform.position.y;
            if (Mathf.Abs(difference) < epsilon)
            {
                return 0;
            }

            return difference > 0 ? -1 : 1;
        });
        for (int i = 0; i < zombieList.Count; i++)
        {
            zombieList[i].transform.SetSiblingIndex(i);
        }
    }

    private void OnDrawGizmos()
    {
        Color[] colors = { Color.green, Color.blue, Color.red };
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            if (spawnAreas[i] != null)
            {
                BoxCollider2D areaCollider = spawnAreas[i].GetComponent<BoxCollider2D>();
                if (areaCollider != null)
                {
                    Gizmos.color = new Color(colors[i].r, colors[i].g, colors[i].b, 0.3f);
                    Gizmos.DrawCube(areaCollider.bounds.center, areaCollider.bounds.size);

                    Gizmos.color = colors[i];
                    Gizmos.DrawWireCube(areaCollider.bounds.center, areaCollider.bounds.size);
                }
            }
        }
    }
}