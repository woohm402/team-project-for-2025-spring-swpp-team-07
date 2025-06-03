public class Upgrade403 : Upgrade {
    public Upgrade403(): base("신나", GetDescription()) {

    }

    private static string GetDescription() {
        return "드리프트를 하지 않아도 부스터가 자동으로 1초에 20%씩 채워집니다.";
    }
}
