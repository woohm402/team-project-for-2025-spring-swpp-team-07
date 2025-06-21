public class Upgrade303 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 1.2f;

    public Upgrade303(KartController kartController): base(303, "레버리지", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float maxSpeed = kartController.GetMaxSpeed();
        return $"최고 속도가\n120% 증가하는 대신,\n주변 차량과 충돌 시\n2초 더 기절합니다.\n\n{maxSpeed} -> {maxSpeed * rate}";
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
        kartController.IncrementDizzyTime(2f);
    }
}
