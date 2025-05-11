import type { Client } from '@notionhq/client';
import { MEMBERS } from '../entities/member';
import { Sprint } from '../entities/sprint';

import type { getSendDailyScrumUsecase } from '../usecases/sendDailyScrumUsecase';
import { ensure } from '../utils/ensure';

export const getDailyScrumNotionHqClientRepository = ({
  client,
}: {
  client: Client;
}): Parameters<typeof getSendDailyScrumUsecase>[0]['notionRepository'] => {
  return {
    getAllTasks: async ({ sprint }) => {
      const sprintProperty = {
        [Sprint.SPRINT_1]: 'Sprint 1 (4/28-5/11)',
        [Sprint.SPRINT_2]: 'Sprint 2 (5/12-5/25)',
        [Sprint.SPRINT_3]: 'Sprint 3 (5/26-6/8)',
        [Sprint.SPRINT_4]: 'Sprint 4 (6/9-6/22)',
      }[sprint];

      const queryResult = await client.databases.query({
        database_id: '1e19614fd0a380089653e1dd33ff6506',
        filter: { property: '스프린트', select: { equals: sprintProperty } },
      });

      return queryResult.results.map((r) => {
        if (r.object !== 'page' || !('properties' in r)) throw new Error('invalid task');
        const titleProperty = r.properties['이름'];
        const assigneeProperty = r.properties['담당자'];
        const expectedScheduleProperty = r.properties['예상 일정'];
        const actualScheduleProperty = r.properties['실제 일정'];
        const expectedSizeProperty = r.properties['예상 시간'];
        const skipProperty = r.properties['스킵'];

        if (
          titleProperty.type !== 'title' ||
          assigneeProperty.type !== 'people' ||
          expectedScheduleProperty.type !== 'date' ||
          actualScheduleProperty.type !== 'date' ||
          expectedSizeProperty.type !== 'number' ||
          skipProperty.type !== 'checkbox'
        )
          throw new Error('invalid notion task');

        return {
          title: titleProperty.title.map((t) => t.plain_text).join(' '),
          assignee: ensure(MEMBERS.find((m) => m.notion === assigneeProperty.people[0].id)),
          expectedSchedule: {
            start: new Date(ensure(expectedScheduleProperty.date).start),
            end: new Date(ensure(ensure(expectedScheduleProperty.date).end)),
          },
          actualSchedule: actualScheduleProperty.date && {
            start: new Date(actualScheduleProperty.date.start),
            end: new Date(ensure(actualScheduleProperty.date.end)),
          },
          skip: skipProperty.checkbox,
          expectedSize: ensure(expectedSizeProperty.number),
        };
      });
    },
  };
};
