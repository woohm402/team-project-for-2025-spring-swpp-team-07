using UnityEngine;
using System.IO;

public static class PlayerDataSaveLoad {
    public static void SaveData(PlayerData playerData) {
        string json = JsonUtility.ToJson(playerData);
        string timestamp = ((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString();
        File.WriteAllText(Path.Combine(Application.persistentDataPath, $"playerDatas-{timestamp}.json"), json);
    }
}
