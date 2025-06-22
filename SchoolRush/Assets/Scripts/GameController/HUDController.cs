using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI checkpointLocationText;
    public TextMeshProUGUI shieldsText;

    public TextMeshProUGUI checkpointNumberText;

    public Image jump;
    public Image drifting;
    public Image boost;

    public Rigidbody playerRigidbody;
    public KartController kartController;
    public CheckpointManager checkpointManager;

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

        // Update checkpoint
        int checkpoint = checkpointManager.GetNextCheckpointID();
        checkpointNumberText.text = checkpoint.ToString();
        switch (checkpoint) {
            case (1) :
                checkpointLocationText.text = "사회대";
                break;
            case (2) :
                checkpointLocationText.text = "아랫공대";
                break;
            case (3) :
                checkpointLocationText.text = "경영대";
                break;
            case (4) :
                checkpointLocationText.text = "노천강당";
                break;
            case (5) :
                checkpointLocationText.text = "관정도서관";
                break;
            case (6) :
                checkpointLocationText.text = "302동";
                break;
        }

        // update status
        Color32 offColor = new Color32(0x50, 0x50, 0x50, 0xff);
        if (kartController.GetCanJump()) jump.color = Color.white;
        else jump.color = offColor;

        if (kartController.GetCanBoost()) boost.color = Color.white;
        else boost.color = offColor;

        if (kartController.GetIsDrifting()) drifting.color = Color.white;
        else drifting.color = offColor;

    }

    public float GetElapsedTime() {
        return this.elapsedTime;
    }
}
