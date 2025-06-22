public class Upgrade102 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.3f;

    public Upgrade102(KartController kartController): base(102, "가속도 증가") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetAccel(kartController.GetAccel() * rate);
    }
}
