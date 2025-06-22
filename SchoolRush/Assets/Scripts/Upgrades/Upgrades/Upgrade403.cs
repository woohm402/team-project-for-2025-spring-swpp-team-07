public class Upgrade403 : Upgrade {
    public Upgrade403(KartController kartController): base(403, "신나") {
        kartController.SetAutoIncrementedDriftPowerPerSecond(50f);
    }
}
