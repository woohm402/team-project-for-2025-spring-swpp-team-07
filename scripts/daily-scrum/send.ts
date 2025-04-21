import { Octokit } from "@octokit/rest";
import { WebClient } from "@slack/web-api";

const SLACK_CHANNEL = import.meta.env.SLACK_CHANNEL;
const SLACK_BOT_TOKEN = import.meta.env.SLACK_BOT_TOKEN;

if (SLACK_CHANNEL === undefined || SLACK_BOT_TOKEN === undefined) {
  throw new Error("SLACK_CHANNEL or SLACK_BOT_TOKEN is not defined");
}

const octokit = new Octokit();
const slackClient = new WebClient(SLACK_BOT_TOKEN);

const members = [
  { github: "woohm402", slack: "U08HG6Q15NK" },
  { github: "cloNoey", slack: "U08HG6P8JJ3" },
  { github: "SummerPea", slack: "U08HZ8HKUDT" },
  { github: "kd00172", slack: "U08HG6NS7B9" },
  { github: "mn39", slack: "U08HZ8KAS5T" },
];

const githubUsernameToSlackMention = (githubUsername: string) => {
  const found = members.find((member) => member.github === githubUsername);
  return found ? `<@${found.slack}>` : `@${githubUsername}`;
};

const openPullRequests = (
  await octokit.rest.pulls.list({
    owner: "SWPP-2025SPRING",
    repo: "team-project-for-2025-spring-swpp-team-07",
    state: "open",
  })
).data.flatMap((pull) => {
  const assignee = pull.assignee ?? pull.user;
  if (assignee === null || pull.draft === true) return [];
  return [
    {
      title: pull.title,
      assignee: assignee.login,
      requestedReviewers: pull.requested_reviewers?.map((r) => r.login) ?? [],
      url: pull.html_url,
      number: pull.number,
    },
  ];
});

const date = new Date();
const dateString = [date.getFullYear(), date.getMonth() + 1, date.getDate()]
  .map((d) => d.toString().padStart(2, "0"))
  .join(".");

const escapeText = (message: string) => {
  const escapingChar = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
  };

  return Object.entries(escapingChar).reduce(
    (acc, [char, escaped]) => acc.replaceAll(char, escaped),
    message,
  );
};

const getMrkdwnField = (title: string, content: string) =>
  ({
    type: "mrkdwn",
    text: [`*${title}*`, content].join("\n"),
  }) as const;

const { ts } = await slackClient.chat.postMessage({
  channel: SLACK_CHANNEL,
  blocks: [
    {
      type: "header",
      text: {
        type: "plain_text",
        text: `📅 ${dateString} 데일리 스크럼`,
        emoji: true,
      },
    },
  ],
  text: `*[ ${dateString} 데일리 스크럼 ]*`,
});

if (!ts) throw new Error();

await slackClient.chat.postMessage({
  channel: SLACK_CHANNEL,
  thread_ts: ts,
  blocks: [
    {
      type: "section",
      text: {
        type: "mrkdwn",
        text: members
          .map((m) => githubUsernameToSlackMention(m.github))
          .join(" "),
      },
    },
    {
      type: "section",
      text: {
        type: "mrkdwn",
        text: "어제 한 일과 오늘 할 일을 스레드 댓글로 공유해 주세요! 병목이나 문제가 있다면 같이 알려주세요.",
      },
    },
    { type: "divider" },
  ],
  text: `어제 한 일과 오늘 할 일을 스레드 댓글로 공유해 주세요! 병목이나 문제가 있다면 같이 알려주세요.`,
});

if (openPullRequests.length !== 0) {
  const { ts } = await slackClient.chat.postMessage({
    channel: SLACK_CHANNEL,
    blocks: [
      {
        type: "header",
        text: { type: "plain_text", text: `리뷰를 기다리고 있어요!` },
      },
    ],
    text: `리뷰를 기다리고 있어요!`,
  });

  if (!ts) throw new Error();

  for (const pr of openPullRequests) {
    await slackClient.chat.postMessage({
      channel: SLACK_CHANNEL,
      text: pr.title,
      blocks: [
        {
          type: "header",
          text: {
            type: "plain_text",
            text: `#${pr.number} ${escapeText(pr.title)}`,
          },
        },
        {
          type: "section",
          fields: [
            getMrkdwnField(
              "Opened by",
              githubUsernameToSlackMention(pr.assignee),
            ),
          ],
        },
        {
          type: "section",
          text: getMrkdwnField(
            "Reviewers",
            pr.requestedReviewers.map(githubUsernameToSlackMention).join(", "),
          ),
          accessory: {
            type: "button",
            text: { type: "plain_text", text: "리뷰하러 가기" },
            value: "go_to_review",
            action_id: "button",
            url: pr.url,
          },
        },
      ],
      thread_ts: ts,
    });
  }
}
