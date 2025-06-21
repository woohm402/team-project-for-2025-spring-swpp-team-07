using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private Camera miniCam;
    private Camera subMiniCam;
    [SerializeField]
    private Camera fullCam;
    private AudioManager am;

    private void Start()
    {
        am = AudioManager.Instance;
        subMiniCam = miniCam.gameObject.transform.GetComponentsInChildren<Camera>()[1];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            am.PlayOneShot(am.mapAudio);

            if (mainCam.isActiveAndEnabled)
            {
                mainCam.enabled = false;
                miniCam.enabled = false;
                subMiniCam.enabled = false;
                fullCam.enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                mainCam.enabled = true;
                miniCam.enabled = true;
                subMiniCam.enabled = true;
                fullCam.enabled = false;
                Time.timeScale = 1;
            }
        }
    }
}
