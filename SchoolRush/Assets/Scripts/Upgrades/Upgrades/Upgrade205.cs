public class Upgrade205 : Upgrade {
    public Upgrade205(): base("스프링", GetDescription()) {

    }

    private static string GetDescription() {
        return "이제 스페이스바를 누르면 위로 10m 점프합니다. 5초에 한 번만 사용할 수 있습니다.";
    }
}
