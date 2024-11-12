using UnityEngine;

[ExecuteInEditMode] // Позволяет выполнять код в режиме редактирования
public class CameraScaler : MonoBehaviour
{
    public Camera targetCamera; // Камера, для которой устанавливается размер
    public float pixelsPerUnit = 100f; // Количество пикселей на юнит

    void Start()
    {
        AdjustOrthographicSize();
    }

#if UNITY_EDITOR
    void Update()
    {
        // В режиме редактора пересчитываем размер при каждом изменении
        AdjustOrthographicSize();
    }
#endif

    void AdjustOrthographicSize()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // По умолчанию использовать основную камеру
        }

        if (targetCamera.orthographic)
        {
            // Высота экрана в пикселях делится на 2 и делится на pixelsPerUnit
            targetCamera.orthographicSize = Screen.height / (2f * pixelsPerUnit);
        }
        else
        {
            Debug.LogWarning("Камера должна быть ортографической для корректного масштабирования.");
        }
    }
}