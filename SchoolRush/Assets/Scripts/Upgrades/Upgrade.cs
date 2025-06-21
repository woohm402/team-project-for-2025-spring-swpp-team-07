public abstract class Upgrade {
    private int id;
    private string title;
    private string description;

    public Upgrade(int id, string title, string description) {
        this.id = id;
        this.title = title;
        this.description = description;
    }

    public string GetTitle() {
        return title;
    }

    public string GetDescription() {
        return description;
    }

    public int GetId() {
        return id;
    }

    public virtual void OnPick() {}
}
