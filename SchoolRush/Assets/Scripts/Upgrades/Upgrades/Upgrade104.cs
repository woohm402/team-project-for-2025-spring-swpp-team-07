public class Upgrade104 : Upgrade {
    public Upgrade104(): base(104, "친환경 정책", GetDescription()) {

    }

    private static string GetDescription() {
        return "도로에 차량이 등장하는 빈도가\n절반으로 줄어듭니다.";
    }
}
