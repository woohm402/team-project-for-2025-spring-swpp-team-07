import type { Octokit } from '@octokit/rest';
import { MEMBERS } from '../entities/member';
import type { getSendDailyScrumUsecase } from '../usecases/sendDailyScrumUsecase';
import { ensure } from '../utils/ensure';

export const getDailyScrumGitHubOctokitRepository = ({
  octokit,
}: {
  octokit: Octokit;
}): Parameters<typeof getSendDailyScrumUsecase>[0]['gitHubRepository'] => {
  return {
    getAllOpenPullRequests: async () => {
      const openPullRequests = (
        await octokit.rest.pulls.list({
          owner: 'SWPP-2025SPRING',
          repo: 'team-project-for-2025-spring-swpp-team-07',
          state: 'open',
        })
      ).data.flatMap((pull) => {
        const assignee = pull.assignee ?? pull.user;
        if (assignee === null || pull.draft === true) return [];
        return [
          {
            title: pull.title,
            assignee: ensure(MEMBERS.find((m) => m.github === assignee.login)),
            reviewers:
              pull.requested_reviewers?.map((r) =>
                ensure(MEMBERS.find((m) => m.github === r.login)),
              ) ?? [],
            url: pull.html_url,
            number: pull.number,
          },
        ];
      });

      return openPullRequests;
    },
  };
};
