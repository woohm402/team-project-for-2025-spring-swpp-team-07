using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;  // TextMeshPro를 사용하는 경우

public class SaveNickname : MonoBehaviour
{

    public TMP_InputField nicknameInputField;

    private const string NicknameKey = "PlayerNickname";

    void Start()
    {
        // 저장된 닉네임 불러오기
        if (PlayerPrefs.HasKey(NicknameKey))
        {
            string savedName = PlayerPrefs.GetString(NicknameKey);
            nicknameInputField.text = savedName;
        }
    }

    public void SaveNicknameClick()
    {
        string nickname = nicknameInputField.text;
        PlayerPrefs.SetString(NicknameKey, nickname);
        PlayerPrefs.Save();  // 명시적으로 저장 (안 해도 되지만 권장)
    }
}
