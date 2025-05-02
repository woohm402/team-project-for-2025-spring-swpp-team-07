import { Octokit } from "@octokit/rest";
import { WebClient } from "@slack/web-api";
import { Client } from "@notionhq/client";
import { getSendDailyScrumUsecase } from "./usecases/sendDailyScrumUsecase";
import { getDailyScrumNotionHqClientRepository } from "./infrastructures/dailyScrumNotionHqClientRepository";
import { getDailyScrumGitHubOctokitRepository } from "./infrastructures/dailyScrumGitHubOctokitRepository";
import { getDailyScrumSlackWebApiPresenter } from "./infrastructures/dailyScrumSlackWebApiPresenter";
import { ensure } from "./utils/ensure";
import { Sprint } from "./entities/sprint";

const SLACK_CHANNEL = ensure(import.meta.env.SLACK_CHANNEL);
const SLACK_BOT_TOKEN = ensure(import.meta.env.SLACK_BOT_TOKEN);
const NOTION_TOKEN = ensure(import.meta.env.NOTION_TOKEN);

void getSendDailyScrumUsecase({
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
  currentSprint: Sprint.SPRINT_1,
}).sendDailyScrum();
