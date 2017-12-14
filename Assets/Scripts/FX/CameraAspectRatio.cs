using UnityEngine;
using System.Collections;

public class CameraAspectRatio : MonoBehaviour
{
    //measured in Unity units
    float preferredHeight = 5.5f;
    float preferredWidth = 10f / 3;

    void Start()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        Camera camera = GetComponent<Camera>();

        camera.orthographicSize = Mathf.Max(preferredHeight, preferredWidth / screenAspect);
    }
}