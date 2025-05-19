using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;

    public Rigidbody playerRigidbody;

    private float elapsedTime = 0f;

    void Start()
    {
        // Initialize UI
        if (timeText != null)
            timeText.text = "00:00";
        if (speedText != null)
            speedText.text = "0.0 m/s";
    }

    void Update()
    {
        // Update elapsed time
        elapsedTime += Time.deltaTime;
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        if (timeText != null)
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Update speed
        float speed = 0f;
        if (playerRigidbody != null)
            speed = playerRigidbody.velocity.magnitude;

        if (speedText != null)
            speedText.text = string.Format("{0:0.0} m/s", speed);
    }

    public float GetElapsedTime() {
        return this.elapsedTime;
    }
}
