using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject HUDUI;
    public GameObject pauseMenuUI;
    public GameObject helpImage;
    private bool isPaused = false;

    [SerializeField]
    private Camera fullCam;
    private AudioManager am;

    private bool isFreezed = false;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        am = AudioManager.Instance;
    }

    void Update()
    {
        if (isFreezed) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            am.PlayOneShot(am.pauseAudio);

            if (isPaused) Resume();
            else if (!fullCam.isActiveAndEnabled) Pause();
        }
    }

    public void Pause()
    {
        am.PlayOneShot(am.pauseAudio);

        helpImage.SetActive(false);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        HUDUI.SetActive(false);
    }

    public void Resume()
    {
        am.PlayOneShot(am.resumeAudio);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        HUDUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void OnHelperImage()
    {
        helpImage.SetActive(true);
    }

    public void OffHelperImate()
    {
        helpImage.SetActive(false);
    }

    public void Freeze(bool b)
    {
        isFreezed = b;
    }
}