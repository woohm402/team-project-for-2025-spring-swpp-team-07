using UnityEngine;

public class Upgrade302 : Upgrade {
    private KartController kartController;
    private static readonly float[] rates = { 1.2f, 1.6f, 2.0f };

    public Upgrade302(KartController kartController): base("창업", GetDescription()) {
        this.kartController = kartController;
    }

    private static string GetDescription() {
        return "최고 속도가 20% 또는 60% 또는 100% 증가합니다.";
    }

    public override void OnPick(){
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rates[Random.Range(0, rates.Length)]);
    }
}
