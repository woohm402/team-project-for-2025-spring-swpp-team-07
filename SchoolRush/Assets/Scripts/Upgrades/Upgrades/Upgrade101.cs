public class Upgrade101 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.3f;

    public Upgrade101(KartController kartController): base("최고 속도 증가", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float maxSpeed = kartController.GetMaxSpeed();
        return $"최고 속도가\n30% 증가합니다.\n\n{maxSpeed} -> {maxSpeed * rate}";
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
    }
}
