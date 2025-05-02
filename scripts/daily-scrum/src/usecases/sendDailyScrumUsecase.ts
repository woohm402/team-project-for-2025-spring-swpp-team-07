import type { PullRequest } from "../entities/github";
import { SPRINT_SCHEDULE_MAP, Sprint } from "../entities/sprint";
import { type Task, TaskStatus } from "../entities/task";
import { ensure } from "../utils/ensure";

export type SendDailyScrumUsecase = {
  sendDailyScrum: () => Promise<void>;
};

export const getSendDailyScrumUsecase = ({
  notionRepository,
  gitHubRepository,
  slackPresenter,
  burndownChartPresenter,
}: {
  notionRepository: {
    getAllTasks: (_: { sprint: Sprint }) => Promise<Task[]>;
  };
  gitHubRepository: {
    getAllOpenPullRequests: () => Promise<PullRequest[]>;
  };
  slackPresenter: {
    sendDailyScrum: (_: {
      tasks: Task[];
      burndownChart: Blob;
    }) => Promise<void>;
    sendAwaitingReviews: (_: { pullRequests: PullRequest[] }) => Promise<void>;
  };
  burndownChartPresenter: {
    getBurndownChartImage: (_: {
      tasks: Pick<
        Task,
        "actualSchedule" | "expectedSchedule" | "expectedSize"
      >[];
      sprint: Sprint;
      now: Date;
    }) => Promise<Blob>;
  };
}): SendDailyScrumUsecase => {
  return {
    sendDailyScrum: async () => {
      const currentSprint = ensure(
        Object.values(Sprint).find(
          (sprint) =>
            SPRINT_SCHEDULE_MAP[sprint].start.getTime() <= Date.now() &&
            SPRINT_SCHEDULE_MAP[sprint].end.getTime() > Date.now(),
        ),
      );
      const tasks = await notionRepository.getAllTasks({
        sprint: currentSprint,
      });

      const burndownChartImage =
        await burndownChartPresenter.getBurndownChartImage({
          tasks,
          sprint: currentSprint,
          now: new Date(),
        });

      await slackPresenter.sendDailyScrum({
        tasks: tasks
          .filter(
            (task) =>
              task.status !== TaskStatus.DONE &&
              task.expectedSchedule.start.getTime() <= Date.now(),
          )
          .toSorted(
            (a, b) =>
              a.expectedSchedule.end.getTime() -
              b.expectedSchedule.end.getTime(),
          ),
        burndownChart: burndownChartImage,
      });

      const openPullRequests = await gitHubRepository.getAllOpenPullRequests();
      if (openPullRequests.length > 0)
        await slackPresenter.sendAwaitingReviews({
          pullRequests: openPullRequests,
        });
    },
  };
};
