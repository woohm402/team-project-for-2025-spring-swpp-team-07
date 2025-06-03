public class Upgrade401 : Upgrade {
    public Upgrade401(): base("최고 속도 증가", GetDescription()) {

    }

    private static string GetDescription() {
        return "최고 속도 50% 증가";
    }
}
