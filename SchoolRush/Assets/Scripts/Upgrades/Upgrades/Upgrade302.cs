public class Upgrade302 : Upgrade {
    public Upgrade302(): base("창업", GetDescription()) {

    }

    private static string GetDescription() {
        return "최고 속도가 20% 또는 60% 또는 100% 증가합니다.";
    }
}
