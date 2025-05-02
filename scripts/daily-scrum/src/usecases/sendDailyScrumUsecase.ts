import type { PullRequest } from "../entities/github";
import type { Member } from "../entities/member";
import { Sprint } from "../entities/sprint";
import { TaskStatus, type Task } from "../entities/task";

export type SendDailyScrumUsecase = {
  sendDailyScrum: () => Promise<void>;
};

export const getSendDailyScrumUsecase = ({
  notionRepository,
  gitHubRepository,
  slackPresenter,
  currentSprint,
}: {
  notionRepository: {
    getAllTasks: (_: { sprint: Sprint }) => Promise<Task[]>;
  };
  gitHubRepository: {
    getAllOpenPullRequests: () => Promise<PullRequest[]>;
  };
  slackPresenter: {
    sendDailyScrum: (_: { tasks: Task[] }) => Promise<void>;
    sendAwaitingReviews: (_: { pullRequests: PullRequest[] }) => Promise<void>;
  };
  currentSprint: Sprint;
}): SendDailyScrumUsecase => {
  return {
    sendDailyScrum: async () => {
      const tasks = await notionRepository.getAllTasks({
        sprint: currentSprint,
      });

      await slackPresenter.sendDailyScrum({
        tasks: tasks
          .filter(
            (task) =>
              task.status !== TaskStatus.DONE &&
              task.schedule.start.getTime() <= Date.now(),
          )
          .toSorted(
            (a, b) => a.schedule.end.getTime() - b.schedule.end.getTime(),
          ),
      });

      const openPullRequests = await gitHubRepository.getAllOpenPullRequests();
      if (openPullRequests.length > 0)
        await slackPresenter.sendAwaitingReviews({
          pullRequests: openPullRequests,
        });
    },
  };
};
