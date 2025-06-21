public class Upgrade404 : Upgrade {
    public Upgrade404(): base(404, "관공이의 가호", GetDescription()) {

    }

    private static string GetDescription() {
        return "주변 차량이 등장하지 않습니다.";
    }
}
