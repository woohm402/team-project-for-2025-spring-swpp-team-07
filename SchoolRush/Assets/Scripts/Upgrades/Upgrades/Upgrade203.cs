public class Upgrade203 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 2f;

    public Upgrade203(KartController kartController): base(203, "오버클럭") {
        this.kartController = kartController;
    }

    public override void OnPick() {
        kartController.SetBoostDuration(kartController.GetBoostDuration() * rate);
    }

}
