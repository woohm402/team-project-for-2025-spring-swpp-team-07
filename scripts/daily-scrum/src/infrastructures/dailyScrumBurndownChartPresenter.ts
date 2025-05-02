import { ChartJSNodeCanvas } from "chartjs-node-canvas";
import type { getSendDailyScrumUsecase } from "../usecases/sendDailyScrumUsecase";

export const getDailyScrumBurndownChartPresenter = (): Parameters<
  typeof getSendDailyScrumUsecase
>[0]["burndownChartPresenter"] => {
  return {
    getBurndownChartImage: async ({ tasks, sprint }) => {
      const chartJSNodeCanvas = new ChartJSNodeCanvas({
        width: 800,
        height: 400,
        backgroundColour: "white",
      });

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
          const date = new Date(
            sprint.start.getTime() + 1000 * 60 * 60 * 24 * i,
          );

          const isNotYet = date.getTime() > Date.now() - 1000 * 60 * 60 * 24;

          return {
            label: [date.getMonth() + 1, date.getDate()]
              .map((d) => d.toString().padStart(2, "0"))
              .join("."),
            expectedValue: tasks.reduce(
              (a, c) =>
                c.expectedSchedule.end.getTime() > date.getTime()
                  ? a + c.expectedSize
                  : a,
              0,
            ),
            actualValue: isNotYet
              ? null
              : tasks.reduce(
                  (a, c) =>
                    c.actualSchedule === null ||
                    c.actualSchedule.end.getTime() > date.getTime()
                      ? a + c.expectedSize
                      : a,
                  0,
                ),
          };
        },
      );

      const buffer = await chartJSNodeCanvas.renderToBuffer({
        type: "line",
        data: {
          labels: dates.map((d) => d.label),
          datasets: [
            {
              label: "Expected",
              data: dates.map((d) => d.expectedValue),
              borderColor: "rgba(0, 255, 0, 0.8)",
              borderDash: [5, 5],
              fill: false,
            },
            {
              label: "Actual",
              data: dates.map((d) => d.actualValue),
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
