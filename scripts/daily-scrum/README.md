# daily-scrum

데일리 스크럼을 보내는 기능. github workflow 를 통해 매일 오전 11시마다 실행된다.

## run

로컬 실행 시:

```env
# .env.local
SLACK_CHANNEL=(타겟 슬랙 채널 ID)
SLACK_BOT_TOKEN=(슬랙봇토큰)
```

```sh
bun send
```
