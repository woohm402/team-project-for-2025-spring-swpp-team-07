public class Upgrade104 : Upgrade {
    private TrafficController trafficController;

    public Upgrade104(TrafficController trafficController): base(104, "친환경 정책") {
        this.trafficController = trafficController;
    }

    public override void OnPick() {
        trafficController.DestroyHalf();
    }
}
