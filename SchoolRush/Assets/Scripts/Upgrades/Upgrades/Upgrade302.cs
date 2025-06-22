using UnityEngine;

public class Upgrade302 : Upgrade {
    private KartController kartController;
    private static readonly float[] rates = { 1.2f, 1.6f, 2.0f };

    public Upgrade302(KartController kartController): base(302, "창업") {
        this.kartController = kartController;
    }

    public override void OnPick(){
        kartController.SetMaxSpeed(kartController.GetMaxSpeed() * rates[Random.Range(0, rates.Length)]);
    }
}
