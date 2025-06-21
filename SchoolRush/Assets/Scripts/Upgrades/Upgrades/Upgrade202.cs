public class Upgrade202 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.5f;

    public Upgrade202(KartController kartController): base(202, "가속도 증가", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float acceleration = kartController.GetAccel();
        return $"가속도가\n50% 증가합니다.\n\n{acceleration} -> {acceleration * rate}";
    }

    public override void OnPick() {
        kartController.SetAccel(kartController.GetAccel() * rate);
    }
}
