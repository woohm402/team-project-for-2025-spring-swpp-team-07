public class Upgrade402 : Upgrade {
    private BGMController bgmController;

    public Upgrade402(BGMController bgmController): base(402, "축제", GetDescription()) {
        this.bgmController = bgmController;
    }

    private static string GetDescription() {
        return "배경음악이 축제 노래로 변경됩니다.";
    }

    public override void OnPick() {
        bgmController.switchToAegukga();
    }
}
