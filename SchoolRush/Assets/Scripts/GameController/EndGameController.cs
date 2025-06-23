using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameController : MonoBehaviour
{
    public GameObject HUDUI;
    public GameObject endGameUI;
    public GameObject finalPanel;
    public TextMeshProUGUI finalTimeText;

    private HUDController hud;
    private bool hasEnded = false;

    [SerializeField]
    private BGMController bgmController;

    void Start()
    {
        endGameUI.SetActive(false);
        hud = FindObjectOfType<HUDController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasEnded && other.CompareTag("Player"))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        bgmController.PlayGameclearBGM();

        hasEnded = true;
        Time.timeScale = 0f;

        HUDUI.SetActive(false);
        endGameUI.SetActive(true);
        finalPanel.SetActive(true);

        if (finalTimeText != null && hud != null)
            finalTimeText.text = hud.timeText.text;

        KartController kartController = FindObjectOfType<KartController>();
        PlayerData playerData = kartController.GetPlayerData();

        #if !UNITY_EDITOR
        playerData.Save((int)(hud.GetElapsedTime() * 1000));
        #endif
    }
}
