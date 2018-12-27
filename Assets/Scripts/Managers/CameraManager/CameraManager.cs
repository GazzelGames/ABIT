using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    CameraController cameraCon;
    bool latchKey;
    private void OnEnable()
    {
        latchKey = true;
        cameraCon = GetComponent<CameraController>();
        //EventManager.pauseEvent += PauseCamera;
        //EventManager.mapEvent += PauseCamera;
    }
    private void OnDisable()
    {
        //EventManager.pauseEvent -= PauseCamera;
        //EventManager.mapEvent -= PauseCamera;
    }

    void PauseCamera()
    {
        latchKey = !latchKey;
        cameraCon.enabled = latchKey;
    }
}
