export enum Sprint {
  SPRINT_1 = 'SPRINT_1',
  SPRINT_2 = 'SPRINT_2',
  SPRINT_3 = 'SPRINT_3',
  SPRINT_4 = 'SPRINT_4',
}

export const SPRINT_SCHEDULE_MAP: Record<Sprint, { start: Date; end: Date }> = {
  [Sprint.SPRINT_1]: {
    start: new Date('2025-04-28'),
    end: new Date('2025-05-11'),
  },
  [Sprint.SPRINT_2]: {
    start: new Date('2025-05-12'),
    end: new Date('2025-05-25'),
  },
  [Sprint.SPRINT_3]: {
    start: new Date('2025-05-26'),
    end: new Date('2025-06-08'),
  },
  [Sprint.SPRINT_4]: {
    start: new Date('2025-06-09'),
    end: new Date('2025-06-22'),
  },
};
