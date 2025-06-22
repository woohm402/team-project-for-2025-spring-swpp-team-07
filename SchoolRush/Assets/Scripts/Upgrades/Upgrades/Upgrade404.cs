public class Upgrade404 : Upgrade {
    TrafficController trafficController;
    AggressiveCarSpawner aggressiveCarSpawner;

    public Upgrade404(TrafficController trafficController, AggressiveCarSpawner aggressiveCarSpawner): base(404, "관공이의 가호") {
        this.trafficController = trafficController;
        this.aggressiveCarSpawner = aggressiveCarSpawner;
    }

    public override void OnPick() {
        trafficController.DestroyAll();
        aggressiveCarSpawner.StopSpawning();
        aggressiveCarSpawner.DestroyAllSpawnedCars();
    }
}
