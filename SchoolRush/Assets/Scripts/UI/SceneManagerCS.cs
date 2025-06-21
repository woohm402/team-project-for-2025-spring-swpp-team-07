using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManagerCS : MonoBehaviour {
    public GameObject helpImage;
    public TMP_InputField nicknameInput;

    public void loadScene(string sceneName) {
        string nickname = nicknameInput.text.Trim();
        if (helpImage.activeInHierarchy) return;
        if (string.IsNullOrEmpty(nickname)) return;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
