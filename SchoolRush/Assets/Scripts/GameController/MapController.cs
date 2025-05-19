using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private Camera miniCam;
    [SerializeField]
    private Camera fullCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mainCam.isActiveAndEnabled)
            {
                mainCam.enabled = false;
                miniCam.enabled = false;
                fullCam.enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                mainCam.enabled = true;
                miniCam.enabled = true;
                fullCam.enabled = false;
                Time.timeScale = 1;
            }
        }
    }
}
