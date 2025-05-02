import { ChartJSNodeCanvas } from "chartjs-node-canvas";
import type { getSendDailyScrumUsecase } from "../../usecases/sendDailyScrumUsecase";
import { SPRINT_SCHEDULE_MAP } from "../../entities/sprint";

type Presenter = Parameters<
  typeof getSendDailyScrumUsecase
>[0]["burndownChartPresenter"];

export const getDailyScrumBurndownChartPresenter = (): Presenter => {
  return {
    getBurndownChartImage: async (params) => {
      const chartJSNodeCanvas = new ChartJSNodeCanvas({
        width: 800,
        height: 400,
        backgroundColour: "white",
      });

      const { labels, actualValues, expectedValues } = getChartData(params);

      const buffer = await chartJSNodeCanvas.renderToBuffer({
        type: "line",
        data: {
          labels,
          datasets: [
            {
              label: "Expected",
              data: expectedValues,
              borderColor: "rgba(0, 255, 0, 0.8)",
              borderDash: [5, 5],
              fill: false,
            },
            {
              label: "Actual",
              data: actualValues,
              borderColor: "blue",
              fill: false,
            },
          ],
        },
        options: {
          scales: {
            y: {
              beginAtZero: true,
              title: { display: true, text: "Remaining Tasks" },
            },
          },
        },
      });
      return new Blob([buffer], { type: "image/png" });
    },
  };
};

export const getChartData = ({
  tasks,
  sprint,
  now,
}: Parameters<Presenter["getBurndownChartImage"]>[0]) => {
  const dates = Array.from(
    { length: 15 },
    (
      _,
      i,
    ): {
      label: string;
      expectedValue: number | null;
      actualValue: number | null;
    } => {
      const schedule = SPRINT_SCHEDULE_MAP[sprint];
      const DAY = 1000 * 60 * 60 * 24;
      const date = new Date(schedule.start.getTime() + DAY * i);

      const isNotYet = date.getTime() > now.getTime() - DAY;

      return {
        label: [date.getMonth() + 1, date.getDate()]
          .map((d) => d.toString().padStart(2, "0"))
          .join("."),
        expectedValue: tasks.reduce(
          (a, c) =>
            c.expectedSchedule.end.getTime() > date.getTime() - DAY
              ? a + c.expectedSize
              : a,
          0,
        ),
        actualValue: isNotYet
          ? null
          : tasks.reduce(
              (a, c) =>
                c.actualSchedule === null ||
                c.actualSchedule.end.getTime() > date.getTime() - DAY
                  ? a + c.expectedSize
                  : a,
              0,
            ),
      };
    },
  );

  return {
    labels: dates.map((date) => date.label),
    expectedValues: dates.map((date) => date.expectedValue),
    actualValues: dates.map((date) => date.actualValue),
  };
};
