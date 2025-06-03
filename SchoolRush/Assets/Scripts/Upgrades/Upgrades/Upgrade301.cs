public class Upgrade301 : Upgrade {
    public Upgrade301(): base("최고 속도 증가", GetDescription()) {

    }

    private static string GetDescription() {
        return "최고 속도 50% 증가";
    }
}
