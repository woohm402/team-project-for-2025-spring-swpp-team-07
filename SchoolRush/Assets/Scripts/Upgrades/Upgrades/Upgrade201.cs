public class Upgrade201 : Upgrade {
    public Upgrade201(): base("최고 속도 증가", GetDescription()) {

    }

    private static string GetDescription() {
        return "최고 속도가 50% 증가합니다.";
    }
}
