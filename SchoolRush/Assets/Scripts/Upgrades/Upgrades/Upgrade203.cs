public class Upgrade203 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 2f;

    public Upgrade203(KartController kartController): base("오버클럭", GetDescription(kartController)) {
        this.kartController = kartController;
    }

    private static string GetDescription(KartController kartController) {
        float boostDuration = kartController.GetBoostDuration();
        return $"부스터 지속시간이\n100% 증가합니다.\n\n{boostDuration} -> {boostDuration * rate}";
    }

    public override void OnPick() {
        kartController.SetBoostDuration(kartController.GetBoostDuration() * rate);
    }

}
