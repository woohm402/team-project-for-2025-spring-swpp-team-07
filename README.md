# SWPP-2025-Spring/Team07 Repository

## 팀

### 팀원

- 우현민 (PM)
- 곽승연 (디자인)
- 조재표 (개발)
- 장호림 (개발, 사운드)
- 문지환 (개발)

### 팀 회의 규칙

- 정기 회의
  - 매주 월요일 소개원실 랩시간에 대면 회의
- 데일리 스크럼
  - 11시~17시 사이에 슬랙 데일리 스크럼 봇에 댓글로 어제 한 일과 오늘 할 일을 올린다
- 회의는 모두 스누메일로 구글 캘린더 인비 보내서 처리한다

## 코드

### 레포지토리 구조

| 폴더 | 설명 |
| --- | --- |
| [wiki](./wiki) | 게임 위키 문서 |
| [scripts](./scripts) | 팀 자동화 도구들 |
| [SchoolRush](./SchoolRush) | Unity 게임 |

### Git 컨벤션

[Trunk Based Development](https://trunkbaseddevelopment.com/) 기반으로 운영합니다. 브랜치는 `main`만 사용합니다. 하지만 main에 직접 push할 경우 코드 리뷰를 하거나 commit에 대한 (이미지 등의) 설명을 자세히 작성하는 게 제한되기에, 임의의 브랜치를 생성하여 `commit` 후 `push` 하고 GitHub 에서 코드 리뷰 후 Squash Merge 합니다.

이때, Trunk Based Development의 규칙에 따라 각 Pull Request의 변경 줄 수를 +300 line 이내로 제한합니다.
