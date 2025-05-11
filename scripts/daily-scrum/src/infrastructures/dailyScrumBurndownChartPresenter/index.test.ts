import { expect, test } from 'bun:test';
import { getChartData } from '.';
import { Sprint } from '../../entities/sprint';

test('getChartData', () => {
  const start = new Date();
  expect(
    getChartData({
      sprint: Sprint.SPRINT_1,
      tasks: [
        {
          expectedSize: 3,
          expectedSchedule: { start, end: new Date('2025-04-28') },
          actualSchedule: null,
        },
        {
          expectedSize: 2,
          expectedSchedule: { start, end: new Date('2025-04-29') },
          actualSchedule: { start, end: new Date('2025-05-01') },
        },
        {
          expectedSize: 4,
          expectedSchedule: { start, end: new Date('2025-05-04') },
          actualSchedule: null,
        },
      ],
      now: new Date('2025-05-03'),
    }),
  ).toStrictEqual({
    actualValues: [9, 9, 9, 9, 7, 7, 7, null, null, null, null, null, null, null, null],
    expectedValues: [9, 6, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0],
    labels: [
      '04.28',
      '04.29',
      '04.30',
      '05.01',
      '05.02',
      '05.03',
      '05.04',
      '05.05',
      '05.06',
      '05.07',
      '05.08',
      '05.09',
      '05.10',
      '05.11',
      '05.12',
    ],
  });
});
