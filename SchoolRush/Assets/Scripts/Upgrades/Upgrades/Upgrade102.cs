public class Upgrade102 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.3f;

    public Upgrade102(KartController kartController): base(102, "가속도 증가", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float acceleration = kartController.GetAccel();
        return $"가속도가\n30% 증가합니다.\n\n{acceleration} -> {acceleration * rate}";
    }

    public override void OnPick() {
        kartController.SetAccel(kartController.GetAccel() * rate);
    }
}
