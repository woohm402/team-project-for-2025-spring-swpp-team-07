public class Upgrade105 : Upgrade {
    private KartController kartController;
    private readonly static int count = 10;

    public Upgrade105(KartController kartController): base(105, "집행유예") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.GiveShields(count);
    }
}
