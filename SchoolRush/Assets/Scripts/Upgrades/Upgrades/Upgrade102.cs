public class Upgrade102 : Upgrade {
    public Upgrade102(): base("가속도 증가", GetDescription()) {

    }

    private static string GetDescription() {
        return "가속도가 30% 증가합니다.";
    }
}
