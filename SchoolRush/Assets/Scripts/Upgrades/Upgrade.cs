using UnityEngine;
public abstract class Upgrade {
    private int id;
    private string title;
    private string description;

    public Upgrade(int id, string title) {
        this.id = id;
        this.title = title;
    }

    public string GetTitle() {
        return title;
    }

    public int GetId() {
        return id;
    }

    public virtual Sprite GetAugment() 
    {
        string spritePath = $"UpgradeImages/{id}";
        return Resources.Load<Sprite>(spritePath);
    }

    public virtual void OnPick() {}
}
