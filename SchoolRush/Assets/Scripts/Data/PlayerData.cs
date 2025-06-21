using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData {
    public string nickname;
    public int totalTime;
    [SerializeField] public List<int> augmentIds;
    [SerializeField] public List<Log> logs;

    public PlayerData(string nickname) {
        this.nickname = nickname;
        augmentIds = new List<int>();
        logs = new List<Log>();
    }

    public void Insert(Vector3 position) {
        this.logs.Add(new Log((int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(), position));
    }

    public void Save(int totalTime) {
        this.totalTime = totalTime;
        PlayerDataSaveLoad.SaveData(this);
    }
}

[System.Serializable]
public class Log {
    public int time;
    public float x, y, z;

    public Log (int time, Vector3 position) {
      this.time = time;
      this.x = position.x;
      this.y = position.y;
      this.z = position.z;
    }
}
