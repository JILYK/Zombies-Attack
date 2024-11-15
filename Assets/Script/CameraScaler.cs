using UnityEngine;

[ExecuteInEditMode]
public class CameraScaler : MonoBehaviour
{
    public Camera targetCamera;
    public float pixelsPerUnit = 100f;

    void Start()
    {
        AdjustOrthographicSize();
    }

#if UNITY_EDITOR
    void Update()
    {
        AdjustOrthographicSize();
    }
#endif

    void AdjustOrthographicSize()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetCamera.orthographicSize = Screen.height / (2f * pixelsPerUnit);
    }
}