public class Upgrade303 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.2f;

    public Upgrade303(KartController kartController): base(303, "레버리지") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
        kartController.IncrementDizzyTime(2f);
    }
}
