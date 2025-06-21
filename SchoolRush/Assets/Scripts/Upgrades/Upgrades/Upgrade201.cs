public class Upgrade201 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.5f;

    public Upgrade201(KartController kartController): base(201, "최고 속도 증가", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float maxSpeed = kartController.GetMaxSpeed();
        return $"최고 속도가\n50% 증가합니다.\n\n{maxSpeed} -> {maxSpeed * rate}";
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
    }
}
