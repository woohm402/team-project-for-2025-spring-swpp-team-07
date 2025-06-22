public class Upgrade401 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.5f;

    public Upgrade401(KartController kartController): base(401, "최고 속도 증가") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
    }
}
