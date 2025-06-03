public class Upgrade203 : Upgrade {
    public Upgrade203(): base("오버클럭", GetDescription()) {

    }

    private static string GetDescription() {
        return "부스터 지속시간이 100% 증가합니다.";
    }
}
