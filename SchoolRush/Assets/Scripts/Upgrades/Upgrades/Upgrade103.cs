public class Upgrade103 : Upgrade {

    private PassengerController pc;

    public Upgrade103(PassengerController pc): base(103, "통행 금지령") {
        this.pc = pc;
    }

    public override void OnPick()
    {
        pc.StopSpawnAndDestroyAll();
    }
}
