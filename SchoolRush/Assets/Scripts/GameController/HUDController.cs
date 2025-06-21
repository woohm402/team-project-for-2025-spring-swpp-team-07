using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI shieldsText;

    public Rigidbody playerRigidbody;
    public KartController kartController;

    private float elapsedTime = 0f;

    void Start() {

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
        float speed = playerRigidbody.velocity.magnitude;
        if (speedText != null)
            speedText.text = string.Format("{0:0.0}", speed);

        // Update shields
        if (shieldsText != null)
            shieldsText.text = $"쉴드 {kartController.GetRemainingShields()}개 남음";
    }

    public float GetElapsedTime() {
        return this.elapsedTime;
    }
}
