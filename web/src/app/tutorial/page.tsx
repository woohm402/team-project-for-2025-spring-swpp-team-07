import mainScene from "./_assets/010.mainScene.png";
import car from "./_assets/020.car.png";
import annoyingCar from "./_assets/030.annoying-car.png";
import people from "./_assets/040.people.png";
import map from "./_assets/050.map.png";
import checkpoint from "./_assets/060.checkpoint.png";
import upgrade from "./_assets/070.upgrade.png";
import control from "./_assets/080.control.png";
import fullmap from "./_assets/090.fullmap.png";
import final from "./_assets/final.avif";
import { Items } from "./_components/Items";

export default async function Home() {
  const items = [
    {
      title: "메인화면",
      image: mainScene,
      description:
        "닉네임을 입력하고 Start를 눌러 주세요.\n\n10등 내에 들면 대시보드에서 기록을 확인할 수 있어요.",
    },
    {
      title: "맵",
      image: map,
      description:
        "관악캠퍼스 정문에서 게임이 시작돼요.\n맵은 도로, 땅, 건물로 구성돼요.\n\n도로 위에서는 1.3배 더 빠르게 달릴 수 있어요.",
    },
    {
      title: "조작법",
      image: control,
      description:
        "방향키로 조작해요.\n\nshift키로 드리프트할 수 있어요.\n\n드리프트를 하면 부스터가 쌓이고, 드리프트가 끝날 때 빨라져요.\n\n도로가 아닌 땅에서는 space 키로 점프할 수 있어요.",
    },
    {
      title: "체크포인트",
      image: checkpoint,
      description:
        "도착지까지 총 6개의 체크포인트가 있어요.\n\n체크포인트 주변에는 노란색 빛 기둥이 보이고,\n\n우측 하단의 미니맵을 통해 체크포인트가 어느 방향에, 얼마나 멀리 있는지 알 수 있어요.",
    },
    {
      title: "증강",
      image: upgrade,
      description:
        "각 체크포인트를 지날 때마다 동료를 한 명씩 태워요.\n동료는 내 차량을 강화시킬 수 있는 증강을 제공해요.\n\n하나만 선택할 수 있으니 신중하게 골라야 해요.",
    },
    {
      title: "전체 지도",
      image: fullmap,
      description:
        "m키로 전체 지도를 열고 확대/축소/이동할 수 있어요.\n지도를 열고 있는 동안은 시간이 흐르지 않아요.\n\n다음으로 가야 하는 체크포인트 위치가 분홍색으로 표시돼요.",
    },
    {
      title: "주변 붕붕이들",
      image: car,
      description:
        "도로를 따라 차들이 다녀요.\n\n지나가는 차와 충돌하면 약간 튕겨나고 잠시 기절해요.",
    },
    {
      title: "공격적인 붕붕이",
      image: annoyingCar,
      description:
        "도로를 따라 다니는 차들과 별개로, 몇 초마다 내 주변에서 차량이 생성되어 나를 향해 돌진해요.\n\n이 차량과 충돌해도 마찬가지로 약간 튕겨나고 잠시 기절해요.",
    },
    {
      title: "주변 행인들",
      image: people,
      description:
        "학교에는 사람(!)들이 다녀요.\n\n사람을 치면 마지막으로 도착한 체크포인트로 강제로 돌아가요.",
    },
    { title: "화이팅!", image: final, description: "재밌습니다" },
  ];

  return (
    <div className="h-dvh flex flex-col">
      <header className="p-4 flex items-center justify-center shadow-md z-20">
        <h1 className="sticky top-0 text-xl text-center font-[MapoDPPA] italic">
          School Rush: 이상현상을 찾아서
        </h1>
      </header>
      <div className="px-6 flex flex-1 flex-col">
        <Items items={items} />
      </div>
    </div>
  );
}
