public abstract class Upgrade {
    private string title;
    private string description;

    public Upgrade(string title, string description) {
        this.title = title;
        this.description = description;
    }

    public string GetTitle() {
        return title;
    }

    public string GetDescription() {
        return description;
    }

    public virtual void OnPick() {}
}
