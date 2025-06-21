public class Upgrade301 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.5f;

    public Upgrade301(KartController kartController): base(301, "최고 속도 증가", GetDescription(kartController)) {
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
