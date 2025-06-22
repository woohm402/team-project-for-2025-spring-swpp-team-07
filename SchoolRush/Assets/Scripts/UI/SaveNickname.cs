using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveNickname : MonoBehaviour {
    public TMP_InputField nicknameInputField;

    private const string NICKNAME_KEY = "PlayerNickname";

    void Start() {
        if (PlayerPrefs.HasKey(NICKNAME_KEY)) {
            string savedName = PlayerPrefs.GetString(NICKNAME_KEY);
            nicknameInputField.text = savedName;
        }

        nicknameInputField.onValueChanged.AddListener(OnNicknameChanged);
    }

    public static string LoadNickname() {
        return PlayerPrefs.GetString(NICKNAME_KEY);
    }

    private void OnNicknameChanged(string value) {
        string trimmed = value.Trim();
        nicknameInputField.text = trimmed;
        PlayerPrefs.SetString(NICKNAME_KEY, trimmed);
        PlayerPrefs.Save();
    }
}
