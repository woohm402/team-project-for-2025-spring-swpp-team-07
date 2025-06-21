using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using System;

public static class PlayerDataSaveLoad {
    public static void SaveData(PlayerData playerData) {
        try {
            string json = JsonUtility.ToJson(playerData);
            string timestamp = ((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString();

            UnityWebRequest request = new UnityWebRequest("https://schoolrush.vercel.app/api/player-data", "POST");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest();
            Debug.Log("Request sent");
        } catch (Exception e) {
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }
}
