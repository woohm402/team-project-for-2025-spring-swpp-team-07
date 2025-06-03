public class Upgrade202 : Upgrade {
    public Upgrade202(): base("가속도 증가", GetDescription()) {

    }

    private static string GetDescription() {
        return "가속도가 50% 증가합니다.";
    }
}
