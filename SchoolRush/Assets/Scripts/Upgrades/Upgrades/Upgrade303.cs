public class Upgrade303 : Upgrade {
    public Upgrade303(): base("레버리지", GetDescription()) {

    }

    private static string GetDescription() {
        return "최고 속도가 120% 증가하는 대신, 주변 차량과 충돌 시 2초 더 기절합니다.";
    }
}
