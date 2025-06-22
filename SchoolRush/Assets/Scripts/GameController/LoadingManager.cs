using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private Camera loadingCam;
    [SerializeField]
    private Image loadingPanel;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    private MapController mapController;
    private PauseMenu pauseMenu;

    private void Awake()
    {
        Time.timeScale = 0;

        mapController = GetComponent<MapController>();
        pauseMenu = GetComponent<PauseMenu>();

        mapController.Freeze(true);
        pauseMenu.Freeze(true);
    }

    public void CountDown(int num)
    {
        loadingText.text = $"{num}";
    }

    public void GameStart()
    {
        Time.timeScale = 1;

        loadingCam.gameObject.Destroy();
        loadingText.gameObject.Destroy();
        loadingPanel.gameObject.Destroy();

        mapController.Freeze(false);
        pauseMenu.Freeze(false);
    }
}
