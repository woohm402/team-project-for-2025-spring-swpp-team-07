public class Upgrade105 : Upgrade {
    private KartController kartController;
    private readonly static int count = 10;

    public Upgrade105(KartController kartController): base("집행유예", GetDescription()) {
        this.kartController = kartController;
    }

    private static string GetDescription() {
        return $"행인 및 주변 차량 충돌 시\n패널티가 최대 {count}회까지 면제됩니다.";
    }

    public override void OnPick() {
        kartController.GiveShields(count);
    }
}
