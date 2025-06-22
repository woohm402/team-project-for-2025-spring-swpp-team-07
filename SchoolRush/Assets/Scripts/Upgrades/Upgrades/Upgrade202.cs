public class Upgrade202 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.5f;

    public Upgrade202(KartController kartController): base(202, "가속도 증가") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetAccel(kartController.GetAccel() * rate);
    }
}
