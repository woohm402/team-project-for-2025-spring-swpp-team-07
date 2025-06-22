public class Upgrade101 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.3f;

    public Upgrade101(KartController kartController): base(101, "최고 속도 증가") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
    }
}
