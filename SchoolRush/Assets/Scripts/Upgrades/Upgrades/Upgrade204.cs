public class Upgrade204 : Upgrade {
    private KartController kartController;
    private readonly static float rate = 2f;

    public Upgrade204(KartController kartController): base(204, "지각이다", GetDescription()) {
        this.kartController = kartController;
    }

    private static string GetDescription() {
        return "택시로 차량을 변경합니다. 최고 속도, 가속도가 모두 100% 더 증가했지만, 폭이 넓어서 다른 차량이나 행인과 더 잘 충돌합니다.";
    }

    public override void OnPick() {
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rate);
        kartController.SetAccel(kartController.GetAccel() * rate);
        kartController.ChangeBikeToTaxi();
    }

}
