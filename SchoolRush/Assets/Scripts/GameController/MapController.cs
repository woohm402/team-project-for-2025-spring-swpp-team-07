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

    [SerializeField]
    private GameObject pauseMenu;
    private AudioManager am;

    private bool isFreezed = false;

    private void Start()
    {
        am = AudioManager.Instance;
        subMiniCam = miniCam.gameObject.transform.GetComponentsInChildren<Camera>()[1];
    }

    private void Update()
    {
        if (isFreezed) return;

        if (Input.GetKeyDown(KeyCode.M) && !pauseMenu.activeSelf)
        {
            am.PlayOneShot(am.mapAudio);

            if (mainCam.isActiveAndEnabled) OpenMap();
            else CloseMap();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!mainCam.isActiveAndEnabled) CloseMap();
        }
    }

    private void OpenMap()
    {
        mainCam.enabled = false;
        miniCam.enabled = false;
        subMiniCam.enabled = false;
        fullCam.enabled = true;
        Time.timeScale = 0;
    }

    private void CloseMap()
    {
        mainCam.enabled = true;
        miniCam.enabled = true;
        subMiniCam.enabled = true;
        fullCam.enabled = false;
        Time.timeScale = 1;
    }

    public void Freeze(bool b)
    {
        isFreezed = b;
    }
}
