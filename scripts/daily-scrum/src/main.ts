import { Client } from '@notionhq/client';
import { Octokit } from '@octokit/rest';
import { WebClient } from '@slack/web-api';
import { getDailyScrumBurndownChartPresenter } from './infrastructures/dailyScrumBurndownChartPresenter';
import { getDailyScrumGitHubOctokitRepository } from './infrastructures/dailyScrumGitHubOctokitRepository';
import { getDailyScrumNotionHqClientRepository } from './infrastructures/dailyScrumNotionHqClientRepository';
import { getDailyScrumSlackWebApiPresenter } from './infrastructures/dailyScrumSlackWebApiPresenter';
import { getSendDailyScrumUsecase } from './usecases/sendDailyScrumUsecase';
import { ensure } from './utils/ensure';

const SLACK_CHANNEL = ensure(import.meta.env.SLACK_CHANNEL);
const SLACK_BOT_TOKEN = ensure(import.meta.env.SLACK_BOT_TOKEN);
const NOTION_TOKEN = ensure(import.meta.env.NOTION_TOKEN);

getSendDailyScrumUsecase({
  notionRepository: getDailyScrumNotionHqClientRepository({
    client: new Client({ auth: NOTION_TOKEN }),
  }),
  gitHubRepository: getDailyScrumGitHubOctokitRepository({
    octokit: new Octokit(),
  }),
  slackPresenter: getDailyScrumSlackWebApiPresenter({
    webClient: new WebClient(SLACK_BOT_TOKEN),
    channel: SLACK_CHANNEL,
  }),
  burndownChartPresenter: getDailyScrumBurndownChartPresenter(),
})
  .sendDailyScrum()
  .catch((err) => {
    console.error(err);
  });
