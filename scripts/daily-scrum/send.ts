import { Octokit } from "@octokit/rest";
import { WebClient } from "@slack/web-api";
import { Client } from "@notionhq/client";

const SLACK_CHANNEL = import.meta.env.SLACK_CHANNEL;
const SLACK_BOT_TOKEN = import.meta.env.SLACK_BOT_TOKEN;
const NOTION_TOKEN = import.meta.env.NOTION_TOKEN;

if (
  SLACK_CHANNEL === undefined ||
  SLACK_BOT_TOKEN === undefined ||
  NOTION_TOKEN === undefined
) {
  throw new Error(
    "SLACK_CHANNEL or SLACK_BOT_TOKEN or NOTION_TOKEN is not defined",
  );
}

const notionClient = new Client({ auth: NOTION_TOKEN });
const octokit = new Octokit();
const slackClient = new WebClient(SLACK_BOT_TOKEN);

const members = [
  {
    github: "woohm402",
    slack: "U08HG6Q15NK",
    notion: "a60a2b22-e58c-4cf8-a100-764f60cac65c",
  },
  {
    github: "cloNoey",
    slack: "U08HG6P8JJ3",
    notion: "d747c822-e583-4eee-9f42-edf7baea585b",
  },
  {
    github: "SummerPea",
    slack: "U08HZ8HKUDT",
    notion: "112d872b-594c-8160-a74b-00026265457c",
  },
  {
    github: "kd00172",
    slack: "U08HG6NS7B9",
    notion: "9cd9e79b-3274-41a5-aa5a-e6b1ee0ff019",
  },
  {
    github: "mn39",
    slack: "U08HZ8KAS5T",
    notion: "96689d8e-1cf1-4bf5-9ec9-caab076bd9f6",
  },
];

const tasks = await notionClient.databases.query({
  database_id: "1e19614fd0a380089653e1dd33ff6506",
  filter: {
    and: [
      { property: "스프린트", select: { equals: "Sprint 1 (4/28-5/11)" } },
      { property: "상태", status: { does_not_equal: "완료" } },
    ],
  },
});

const targetTasks = tasks.results
  .flatMap((r) => {
    if (r.object !== "page" || !("properties" in r)) throw new Error();
    const titleProperty = r.properties["이름"];
    const assigneeProperty = r.properties["담당자"];
    const expectedScheduleProperty = r.properties["예상 일정"];

    if (
      titleProperty.type !== "title" ||
      assigneeProperty.type !== "people" ||
      expectedScheduleProperty.type !== "date" ||
      expectedScheduleProperty.date === null ||
      expectedScheduleProperty.date.end === null
    )
      throw new Error();

    const assignee = members.find(
      (m) => m.notion === assigneeProperty.people[0].id,
    );

    if (!assignee) throw new Error();

    return {
      title: titleProperty.title.map((t) => t.plain_text).join(" "),
      assignee,
      schedule: {
        start: expectedScheduleProperty.date.start,
        end: expectedScheduleProperty.date.end,
      },
    };
  })
  .filter((r) => Date.now() >= new Date(r.schedule.start).getTime());

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
        text: "어제 한 일과 오늘 할 일을 스레드 댓글로 공유해 주세요! 병목이나 문제가 있다면 같이 알려주세요.",
      },
    },
    { type: "divider" },
    {
      type: "section",
      text: { type: "plain_text", text: "할 일" },
    },
    {
      type: "section",
      text: {
        type: "mrkdwn",
        text: targetTasks
          .sort(
            (a, b) =>
              new Date(a.schedule.end).getTime() -
              new Date(b.schedule.end).getTime(),
          )
          .map((task) =>
            [
              `*\`~${task.schedule.end.slice("2025-".length).replace("-", ".")}\`*`,
              githubUsernameToSlackMention(task.assignee.github),
              task.title,
            ].join(" "),
          )
          .join("\n"),
      },
    },
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
