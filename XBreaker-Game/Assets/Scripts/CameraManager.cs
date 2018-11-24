using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    void Start()
    {
        float baseScreenH = 2560;
        float baseScreenW = 1440;
        float baseCameraSize = 5.3f;

        Camera.main.orthographicSize = (Screen.height * baseCameraSize * baseScreenW) / (baseScreenH * Screen.width);
        Debug.Log("Screen_ Width - " + Screen.width + " Height - " + (float)Screen.height);
        Debug.Log("Camera_ Width - " + Camera.main.pixelWidth + " Height - " + Camera.main.pixelHeight);

    }
  
}

