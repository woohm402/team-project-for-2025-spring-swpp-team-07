public class Upgrade204 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 2f;

    public Upgrade204(KartController kartController): base(204, "지각이다") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
        kartController.SetAccel(kartController.GetAccel() * rate);
        kartController.ChangeBikeToTaxi();
    }

}
