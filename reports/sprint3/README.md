# Sprint 3 계획 회의 보고서

> Team 07 우현민, 곽승연, 조재표, 장호림, 문지환

<br/><br/>

## 스프린트 백로그 및 태스크 선정 이유

프로젝트 중간발표 당시 Sprint 3에서의 목표는 증강 작업만 마무리하고 QA 단계로 넘어가는 것이었으나, 앞선 Sprint 1과 Sprint 2에서 생각보다 많은 작업이 밀렸기에 Sprint 3에서 증강 및 맵 작업을 마무리하고 Sprint 4에서 QA로 넘어가는 것으로 Sprint 3 목표가 조정되었습니다.

이런 배경에서 아래 태스크들을 선정했습니다. 아래 섹션에서는 개괄적인 내용을 다루며, 전체 태스크 목록 및 선정 이유는 아래 태스크 할당 표 섹션에서 확인할 수 있습니다.

#### 증강 시스템 구현

일정대로 Sprint 2에서 증강 목록을 작성하여 팀 내 리뷰를 거쳤고 그 결과 [이렇게](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/e3e5783c13dc8f95a0aa6a536fde2771e55840ca/wiki/4.AUGMENTS.md?plain=1#L1) 증강 목록을 확정할 수 있었습니다. Sprint 3에서는 핵심 게임 시스템을 모두 구현해야 Sprint 4에서 QA 및 디자인 정리 작업을 진행할 수 있기에 본 스프린트에서 모든 증강을 완성하는 것을 목표로 잡았습니다.

특히 증강은 아키텍처를 잘 잡아 두는 게 중요하면서도 구현해야 하는 코드의 양이 많았기에 크게 태스크를 아래와 같이 분리했습니다.

- 초기 아키텍처를 잡고 예시를 위해 증강 한 개 구현하기 (장호림, 우현민)
- 잡힌 아키텍처 위에서 속도 및 가속도 관련 증강 구현하기 (문지환)
- 잡힌 아키텍처 위에서 속도/가속도와 관련 없는 나머지 모든 증강 구현하기 (우현민)

#### 동료, 차량2(택시), 행인 에셋 제작

에셋 작업이 아직 완료되지 않은 상태였는데, 마찬가지로 Sprint 4에서는 에셋을 다듬는 정도의 작업만 해야 하고 에셋 작업은 큰 틀에서 Sprint 3에는 완료하는 것을 목표로 잡았습니다. 따라서 에셋 제작 태스크를 모두 완료하는 것으로 일정을 잡았는데, 지금까지의 작업 속도를 고려했을 때 만들기 쉬운 에셋을 사용하는 게 좋겠다고 생각했습니다. 이에 따라 Minecraft 의 디자인을 오마주하여 모든 에셋을 네모낳게 만드는 방향으로 가기로 하였습니다.

#### 맵 고도화 (수목 및 지형지물)

마찬가지로 아직 Map을 제대로 적용하지 못한 상태였는데, Sprint 4에서는 맵의 마이너 개선사항을 다듬고 디테일을 잡는 정도의 작업만 진행해야 했기에 맵 작업을 Sprint 3에는 모두 완료하는 것으로 계획했습니다.

#### 기타

기타 계획 회의 준비 및 진행, 회고 회의 준비 및 진행, 스프린트 보고서 작성 태스크 역시 개인별 가용 시간에 고려해야 했기에 태스크로 잡아뒀습니다.

<br/><br/>

## 개인별 가용시간 체크

| 조원 | 역할 | 가용시간 |
| --- | --- | --- |
| 우현민 | PM | 15시간 |
| 곽승연 | 디자인 | 20시간 |
| 조재표 | 맵제작 | 20시간 |
| 장호림 | 사운드, 개발 | 18시간 |
| 문지환 | 개발 | 25시간 |

<br/><br/>

## 태스크 할당 표

| 태스크 | 담당자 | 스토리 포인트 (소요시간) |
| --- | --- | --- |
| 스프린트 3 계획 회의 준비 | 우현민 | 1 |
| 스프린트 3 계획 회의 진행 | 전체 | 1 |
| 동료,행인 에셋들 리서치 및 적용 | 곽승연 | 2 |
| 증강 시스템 초안 및 101 증강 구현 (선택 ui, 각각 항목 뜨는 거, 코드레벨에서 틀) | 장호림 | 10 |
| 택시 제작 및 적용 | 곽승연 | 5 |
| 최종 증강: 비행 시스템 구현 | 문지환 | 2 |
| 주변 차량 동작 (교차로 등) 자연스럽게 처리 | 문지환 | 5 |
| 맵 고도화: 수목 | 조재표 | 10 |
| 속도 및 부스터 등 카트 조작 관련 증강 구현 | 문지환 | 8 |
| 카트와 무관한 증강들 구현 | 우현민 | 10 |
| 맵 고도화: 도로 | 조재표 | 10 |
| 스프린트 3 회고 회의 준비 & 끝나고 보고서 작성 | 우현민 | 3 |
| 스프린트 3 회고 회의 진행 | 전체 | 1.5 |

<br/><br/><br/><br/><br/>

# Sprint 3 일별 태스크 진행 요약

> Team 07 우현민, 곽승연, 조재표, 장호림, 문지환

<br/><br/>

## 코드 및 에셋 커밋 기록

Sprint 3 기간인 5/26부터 6/08까지의 커밋 내역은 아래 링크에서 확인할 수 있습니다.

[(GitHub) 5/26 ~ 6/08 전체 커밋 기록](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/commits/main/?since=2025-05-26&until=2025-06-08)

<br/><br/>

## 번다운 차트 및 진행 내역

#### 번다운 차트

![chart](./assets/burndown-chart.png)

회고 회의에서 번다운 차트를 리뷰했을 때, 지난 Sprint 1과 Sprint 2보다는 많이 발전한 모습을 확인할 수 있었고 팀 문화가 좋아지고 있음을 팀원들끼리 확인했습니다.

#### 세부 진행 내역

| 태스크 | 담당자 | 예상 시간 | 실제 시간 | 예상 일정 | 실제 일정 | 관련 링크 |
| --- | --- | --- | --- | --- | --- | --- |
| 스프린트 3 계획 회의 준비 | 우현민 | 1 | 2025년 5월 26일 → 2025년 5월 26일 | 0.5 | 2025년 5월 26일 → 2025년 5월 26일 | [Slack File](https://2025springswppimo.slack.com/files/U08HG6Q15NK/F08U1MBN7MY/250526_____________________________3___________________________________.pdf) |
| 스프린트 3 계획 회의 진행 | 전체 | 1 | 2025년 5월 26일 → 2025년 5월 26일 | 1 | 2025년 5월 26일 → 2025년 5월 26일 | [Slack File](https://2025springswppimo.slack.com/files/U08HG6Q15NK/F08U1MBN7MY/250526_____________________________3___________________________________.pdf) |
| 증강 시스템 초안 및 101 증강 구현 (선택 ui, 각각 항목 뜨는 거, 코드레벨에서 틀) | 장호림,우현민 | 10 | 2025년 5월 26일 → 2025년 5월 29일 | 10 | 2025년 5월 26일 → 2025년 6월 3일 | [GitHub PR#68](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/68) |
| 행인 에셋들 리서치 및 적용 | 곽승연 | 2 | 2025년 5월 26일 → 2025년 5월 26일 | 2 | 2025년 5월 26일 → 2025년 5월 26일 | [GitHub PR#69](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/69) |
| 맵 고도화: 수목 | 조재표 | 10 | 2025년 6월 1일 → 2025년 6월 3일 | 10 | 2025년 5월 26일 → 2025년 5월 27일 | [GitHub PR#72 Commit](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/72/commits/3d8137ac74dbc044b71c8693823bac5a1d1a6dff) |
| 택시 제작 및 적용 | 곽승연 | 5 | 2025년 5월 26일 → 2025년 6월 1일 | 6 | 2025년 5월 30일 → 2025년 5월 30일 | [GitHub PR#73](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/73) |
| 최종 증강: 비행 시스템 구현 | 문지환 | 2 | 2025년 5월 29일 → 2025년 5월 29일 | 2 | 2025년 6월 2일 → 2025년 6월 2일 | [GitHub PR#75](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/75) |
| 동료 에셋들 리서치 및 적용 | 곽승연 | 7 | 2025년 6월 3일 → 2025년 6월 5일 | 12 | 2025년 6월 3일 → 2025년 6월 7일 | [GitHub PR#84](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/84) |
| 스프린트 3 회고 회의 준비 & 끝나고 보고서 작성 | 우현민 | 3 | 2025년 6월 7일 → 2025년 6월 8일 | 6 | 2025년 6월 6일 → 2025년 6월 8일 | [Slack File](https://2025springswppimo.slack.com/files/U08HG6Q15NK/F090VF6M1A5/250608_____________________________3__________________________.pdf) |
| 카트와 무관한 증강들 구현 (105, 402) | 우현민 | 4 | 2025년 6월 1일 → 2025년 6월 7일 | 6 | 2025년 6월 7일 → 2025년 6월 7일 | GitHub PR [#76](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/76), [#80](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/80) |
| 맵 고도화: 도로 | 조재표 | 10 | 2025년 6월 4일 → 2025년 6월 7일 | 10 | 2025년 6월 7일 → 2025년 6월 8일 | |
| 스프린트 3 회고 회의 진행 | 전체 | 1.5 | 2025년 6월 8일 → 2025년 6월 8일 | 0.5 | 2025년 6월 8일 → 2025년 6월 8일 | [Slack File](https://2025springswppimo.slack.com/files/U08HG6Q15NK/F090VF6M1A5/250608_____________________________3__________________________.pdf) |
| 주변 차량 동작 (교차로 등) 자연스럽게 처리 | 문지환 | 5 | 2025년 5월 29일 → 2025년 5월 31일 | - |
| 속도 및 부스터 등 카트 조작 관련 증강 구현 | 문지환 | 8 | 2025년 6월 1일 → 2025년 6월 7일 | - |
| 주변 차량 관련 증강 구현 (104, 404) | 우현민 | 3 | 2025년 6월 1일 → 2025년 6월 7일 | - |
| 행인 관련 증강 구현 (103) | 우현민 | 3 | 2025년 6월 1일 → 2025년 6월 7일 | - |

<br/><br/>

## 페어 프로그래밍 기록

| 내용 | Driver | Navigator | 날짜 | 시간 | 작업 결과물 | 증빙 |
| --- | --- | --- | --- | --- | --- | --- |
| `주변 행인 에셋 제작 및 적용` 중 에셋 제작 | 곽승연 | 우현민 | 05-26 | 18:01-18:49 | [Slack File](https://2025springswppimo.slack.com/archives/C08KVGJU4H4/p1748253004155519?thread_ts=1748251874.388379&cid=C08KVGJU4H4) | [Slack Thread](https://2025springswppimo.slack.com/archives/C08KVGJU4H4/p1748264377655899?thread_ts=1748251874.388379&cid=C08KVGJU4H4) |
| `증강 시스템 기반 구현` 중 ui 및 아키텍처 초안 작업 | 장호림 | 조재표 | 05-26 | 19:00-20:00 | [GitHub PR#68](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/68) | [Slack Image](https://2025springswppimo.slack.com/archives/C08KVGJU4H4/p1748271992883079?thread_ts=1748251874.388379&cid=C08KVGJU4H4) |
| `맵 고도화: 수목 생성` 중 수목 작업 일부 커밋 | 조재표 | 장호림 | 05-26 | 20:00-21:00 | [GitHub PR#72 Commit](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/72/commits/3d8137ac74dbc044b71c8693823bac5a1d1a6dff) | [Slack Image](https://2025springswppimo.slack.com/archives/C08KVGJU4H4/p1748271992883079?thread_ts=1748251874.388379&cid=C08KVGJU4H4) |

# Sprint 2 회고 회의 보고서

> Team 07 우현민, 곽승연, 조재표, 장호림, 문지환

<br/><br/>

## KPT 정리

<table>
  <thead>
    <tr>
      <th>사람</th>
      <th>Keep</th>
      <th>Problem</th>
      <th>Try</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>우현민</td>
      <td>-</td>
      <td>[프로세스] 문지환, 장호림 학생 연락이 잘 안 됨 <br/> [프로세스] 맵 구현이 늦어져 병목 걸린 태스크 존재 <br/> [프로세스] 아직 게임 로직들이 파편화되어 있음</td>
      <td>[프로세스] 병목이 문제되지 않도록, 중요한 태스크들은 태스크 기한을 잘 지키는 사람에게만 할당하기 <br/> [프로세스] 대면 회의를 길게 잡고 태스크 빠르게 진행하기</td>
    </tr>
    <tr>
      <td>곽승연</td>
      <td>[태스크] 무료 에셋 사용으로 시간 단축 <br/> [태스크] 스케줄을 지키면서 열심히 했음</td>
      <td>[태스크] 예상 스케줄보단 미뤄짐</td>
      <td>[개선] 애니메이션이나 자세 등 디테일한 작업을 진행하고 싶음</td>
    </tr>
    <tr>
      <td>조재표</td>
      <td>[태스크] 책임감을 가지고 열심히 진행</td>
      <td>[태스크] 큰 오픈소스에 대한 러닝 커브</td>
      <td>[태스크] 시간이 걸리더라도 필요한 거는 확실하게 공부하고 들어가기</td>
    </tr>
    <tr>
      <td>장호림</td>
      <td>-</td>
      <td>[태스크] 다른 사람 코드 이해에 시간 소요</td>
      <td>[개선] 더 모듈화된 구조</td>
    </tr>
    <tr>
      <td>문지환</td>
      <td colSpan="3">회고 회의 불참으로 작성 내용이 없습니다.</td>
    </tr>
  </tbody>
</table>

<br/><br/>

## 프로덕트 백로그 우선순위 조정 기록

- ***`동료,행인 에셋들 리서치 및 적용`*** 태스크의 경우 기존에 동일한 에셋을 사용하려고 했으나 페어프로그래밍 중 별도 에셋을 사용하는 게 낫겠다고 판단되어, 별도 에셋이므로 태스크를 ***`동료 에셋들 리서치 및 적용`*** 과 ***`행인 에셋들 리서치 및 적용`*** 으로 분리하였습니다.
- 문지환 학생이 담당했던 ***`속도 및 부스터 등 카트 조작 관련 증강 구현`*** 및 ***`주변 차량 동작 (교차로 등) 자연스럽게 처리`*** 태스크가 완료되지 않았습니다. 관련하여 팀원들과 연락도 잘 되지 않아 병목이 걸려 있던 후속 작업인 ***`주변 차량 관련 증강 구현 (104, 404)`*** 및 ***`행인 관련 증강 구현 (103)`*** 이 진행되지 못했습니다. 해당 작업들은 Sprint 4에서 담당자를 변경하여 진행 예정입니다.

<br/><br/>

## 총평

모든 팀원들이 연락이 잘 될 것으로 기대하고 약속했고, 문제가 있으면 연락을 통해 우선순위를 잘 조정할 수 있을 것으로 기대했으나 그렇지 않았던 점이 지금까지와 같이 이번 스프린트에서도 문제로 드러났습니다. 이전 스프린트까지는 다음엔 잘 해 보는 것으로 팀원들끼리 약속했었으나, Sprint 2와 Sprint 3 두 번의 스프린트를 거치며 이런 약속이 동작하지 않는다는 것을 확인하였고, 특히 주요 태스크를 담당하는 팀원이 이런 상황일 경우 이 점이 팀 전체의 생산성을 떨어트린다는 것을 다시금 확인하였습니다. 결국 마지막 스프린트인 Sprint 4에서는 연락이 잘 되는 팀원들만 주요 태스크를 담당하는 것으로 결정했습니다.

프로젝트가 후반부로 접어들면서 펼쳐둔 코드들을 정리하고 마무리하는 절차를 진행해야 하게 되었습니다. 그동안 팀으로 활동하며 팀원들의 성향이나 특징도 서로 파악하였고, 일하는 방식에 대해서도 서로 이해하고 있기에 Sprint 4에서는 팀이 낼 수 있는 최대의 퍼포먼스를 낼 수 있을 걸로 기대한다고 논의되었습니다.

다른 과목들의 종강도 다가오고 있기에 본 프로젝트에 할애할 수 있는 시간이 점점 늘어나고 있고, Sprint 4에서는 최고의 역량을 발휘하여 한 학기 프로젝트를 잘 마무리하는 것을 목표하는 것으로 회고가 마무리되었습니다.

<br/><br/><br/><br/><br/>

# 스프린트 추가내용

Sprint 3에서의 추가 작성 내용은 디자인 패턴, 테스팅입니다.

## 디자인 패턴

### UpgradeManager: Strategy Pattern

증강 시스템은 **매 체크포인트마다 세 개의 증강 중 하나를 고르며, 고른 증강의 효과가 적용된다** 라는 공통적인 특징을 가지면서도 체크포인트별로 떠야 하는 증강의 목록이나 빈도가 달라져야 했습니다. 따라서 각 체크포인트별로 다른 동작을 할 수 있도록 `Strategy Pattern`을 적용하였고, switch 문을 통해 구현했습니다.

```csharp
List<Upgrade> upgrades = new List<Upgrade>();

switch(checkpoint) {
    case 1:
        upgrades.AddRange(/* strategy for checkpoint 1 */);
        break;
    case 2:
        upgrades.AddRange(/* strategy for checkpoint 2 */);
        break;
    // ...
}
```

이를 통해 각 체크포인트별로 해야 하는 동작을 명확하게 구현할 수 있었고, 코드만으로 실제 동작을 표현할 수 있다는 Strategy Pattern의 장점 역시 챙길 수 있었습니다. 상세 코드는 [`UpgradeManager.cs`](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/c685612de59d6c358d99eec7278077b4bb00f50b/SchoolRush/Assets/Scripts/Upgrades/UpgradeManager.cs#L34-L67)에서 확인할 수 있습니다.

### Controllers: Singleton Pattern

`KartController`, `GameController`, `HUDController`, `UpgradeManager` 등 Controller 들은 시스템을 통틀어 유일함이 보장되는 데이터를 저장하고 있어야 합니다. 가령 카드의 속도, 현재 소요 시간, 현재 뽑은 증강 목록 등입니다. 대표적으로 `KartController`는 이렇게 카드의 주행 로그를 담고 있는 [`PlayerData`](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/c685612de59d6c358d99eec7278077b4bb00f50b/SchoolRush/Assets/Scripts/Kart/KartController.cs#L28)를 담고 있는데, 이 데이터는 게임 전체를 통틀어 하나만 존재해야만 버그가 발생하지 않습니다. 따라서 singleton pattern 을 활용하여 단 하나의 인스턴스만 생성해두고 다른 곳들에서는 레퍼런스하여 사용 ([예시](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/c685612de59d6c358d99eec7278077b4bb00f50b/SchoolRush/Assets/Scripts/GameController/EndGameController.cs#L40))하는 식으로 구현했습니다.

<br/><br/>

## 테스팅

### Daily Scrum: Burn Down Chart Module

Sprint 1에서 Daily Scrum 봇에 burndown chart 를 구현할 때 [테스트](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/c47fef22b7ec7392f314330dd9eceadfc4ce9b82/scripts/daily-scrum/src/infrastructures/dailyScrumBurndownChartPresenter/index.test.ts#L1-L51)를 작성하여 구현하였습니다. 외부 솔루션을 사용하지 않고 번다운 차트 이미지를 직접 생성하여 구현하다 보니 오늘이 며칠이고 몇 스프린트인지, 각 태스크의 완료 시점이 언제인지, 각 태스크의 사이즈가 얼마인지 등에 따라 달라지는 차트 모양을 직접 계산해야 했는데, 이 스펙이 다소 복잡하여 테스트를 구축했습니다. 또한 이 테스트는 GitHub Actions 를 활용하여 [이렇게](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/62c66f521936eb44f58769bf5c0c50b9ae146363/.github/workflows/daily-scrum-ci.yml#L25) CI에 통합했습니다.

[PR#22](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/22)에서 구현했습니다.

### Unity: RandomPicker

증강 시스템 기획에는 [다음](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/e243c6b3c9a8cf8de9688b77ef1d89bbca76990c/wiki/4.AUGMENTS.md?plain=1#L21)과 같이 `101, 102 중 한 개, 103, 104, 105 중 두 개가 떠야 한다` 같은 스펙이 존재합니다. 증강이 랜덤하게 뜬다는 사실은 게임을 플레이하는 유저들에게 중요한 재미 요소이기 때문에 이 지점을 테스트해야 했습니다. Unity 가 직접 지원하는 테스트 도구도 있었으나, 단위 테스트이니만큼 스크립트가 unity에 종속되지 않길 바랐고, 또한 CI에 원활하게 통합하기 위해 `dotnet` CLI를 활용하는 방향으로 구축했습니다.

테스트 코드는 [/UnitTests/RandomPicker.test.cs](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/blob/e243c6b3c9a8cf8de9688b77ef1d89bbca76990c/UnitTests/RandomPicker.test.cs#L1-L64) 에서 확인할 수 있습니다.

이 중 세 번째 테스트케이스: `Pick_ShouldUniform` 이 다소 특이한데, *정해진 대로 동작해야 한다* 가 아닌 *랜덤하게 동작해야 한다* 를 증명해야 했기에 5개의 아이템들 중 3개를 뽑는 작업을 10000회 반복한 다음 각 아이템이 5700회 초과 6300회 미만으로 떴다면 랜덤하다고 볼 수 있다고 가정했습니다. 따라서 테스트코드의 검증 로직이 아래와 같이 구현되었습니다.

```csharp
foreach (var count in counts.Values) {
    Assert.That(count, Is.GreaterThan(5700).And.LessThan(6300));
}
```

[PR#83](https://github.com/SWPP-2025SPRING/team-project-for-2025-spring-swpp-team-07/pull/83) 에서 구현했습니다.
