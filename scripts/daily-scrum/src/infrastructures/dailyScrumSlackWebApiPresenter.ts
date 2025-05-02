import type { WebClient } from '@slack/web-api';
import type { getSendDailyScrumUsecase } from '../usecases/sendDailyScrumUsecase';

import type { Member } from '../entities/member';
import { ensure } from '../utils/ensure';

export const getDailyScrumSlackWebApiPresenter = ({
  webClient,
  channel,
}: {
  webClient: WebClient;
  channel: string;
}): Parameters<typeof getSendDailyScrumUsecase>[0]['slackPresenter'] => {
  return {
    sendDailyScrum: async ({ tasks, burndownChart }) => {
      const date = new Date();
      const dateString = [date.getFullYear(), date.getMonth() + 1, date.getDate()]
        .map((d) => d.toString().padStart(2, '0'))
        .join('.');
      const headPostResult = await webClient.chat.postMessage({
        channel,
        blocks: [
          {
            type: 'header',
            text: {
              type: 'plain_text',
              text: `📅 ${dateString} 데일리 스크럼`,
              emoji: true,
            },
          },
        ],
        text: `*[ ${dateString} 데일리 스크럼 ]*`,
      });

      await webClient.files.uploadV2({
        file: Buffer.from(await burndownChart.arrayBuffer()),
        filename: `${dateString}-burndown-chart.png`,
        channel_id: channel,
        thread_ts: ensure(headPostResult.ts),
      });

      await webClient.chat.postMessage({
        channel,
        thread_ts: ensure(headPostResult.ts),
        blocks: [
          {
            type: 'section',
            text: {
              type: 'mrkdwn',
              text: '어제 한 일과 오늘 할 일을 스레드 댓글로 공유해 주세요! 병목이나 문제가 있다면 같이 알려주세요.',
            },
          },
          { type: 'divider' },
          {
            type: 'section',
            text: { type: 'plain_text', text: '오늘 예정된 할 일' },
          },
          {
            type: 'section',
            text: {
              type: 'mrkdwn',
              text: tasks
                .map((task) =>
                  [
                    `*\`~${[task.expectedSchedule.end.getMonth() + 1, task.expectedSchedule.end.getDate()].map((s) => s.toString().padStart(2, '0')).join('.')}\`*`,
                    toSlackMention(task.assignee),
                    task.title,
                  ].join(' '),
                )
                .join('\n'),
            },
          },
        ],
        text: '어제 한 일과 오늘 할 일을 스레드 댓글로 공유해 주세요! 병목이나 문제가 있다면 같이 알려주세요.',
      });
    },

    sendAwaitingReviews: async ({ pullRequests }) => {
      const headPostResult = await webClient.chat.postMessage({
        channel,
        blocks: [
          {
            type: 'header',
            text: { type: 'plain_text', text: '리뷰를 기다리고 있어요!' },
          },
        ],
        text: '리뷰를 기다리고 있어요!',
      });

      for (const pr of pullRequests) {
        await webClient.chat.postMessage({
          channel,
          text: pr.title,
          blocks: [
            {
              type: 'header',
              text: {
                type: 'plain_text',
                text: `#${pr.number} ${escapeText(pr.title)}`,
              },
            },
            {
              type: 'section',
              fields: [getMrkdwnField('Opened by', toSlackMention(pr.assignee))],
            },
            {
              type: 'section',
              text: getMrkdwnField('Reviewers', pr.reviewers.map(toSlackMention).join(', ')),
              accessory: {
                type: 'button',
                text: { type: 'plain_text', text: '리뷰하러 가기' },
                value: 'go_to_review',
                action_id: 'button',
                url: pr.url,
              },
            },
          ],
          thread_ts: ensure(headPostResult.ts),
        });
      }
    },
  };
};

const escapeText = (message: string) => {
  const escapingChar = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
  };

  return Object.entries(escapingChar).reduce(
    (acc, [char, escaped]) => acc.replaceAll(char, escaped),
    message,
  );
};

const getMrkdwnField = (title: string, content: string) =>
  ({
    type: 'mrkdwn',
    text: [`*${title}*`, content].join('\n'),
  }) as const;

const toSlackMention = (member: Member) => {
  return `<@${member.slack}>`;
};
