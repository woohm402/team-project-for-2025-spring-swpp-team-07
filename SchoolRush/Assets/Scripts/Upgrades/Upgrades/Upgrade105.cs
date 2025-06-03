public class Upgrade105 : Upgrade {
    public Upgrade105(): base("집행유예", GetDescription()) {

    }

    private static string GetDescription() {
        return "행인 및 주변 차량 충돌 시 패널티가 최대 10회까지 면제됩니다.";
    }
}
