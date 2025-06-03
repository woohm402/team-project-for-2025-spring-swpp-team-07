public class Upgrade204 : Upgrade {
    public Upgrade204(): base("지각이다", GetDescription()) {

    }

    private static string GetDescription() {
        return "택시로 차량을 변경합니다. 최고 속도, 가속도가 모두 100% 더 증가했지만, 폭이 넓어서 다른 차량이나 행인과 더 잘 충돌합니다.";
    }
}
