public class Upgrade402 : Upgrade {
    private BGMController bgmController;

    public Upgrade402(BGMController bgmController): base(402, "축제") {
        this.bgmController = bgmController;
    }

    public override void OnPick() {
        bgmController.switchToAegukga();
    }
}
