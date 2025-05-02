import type { Member } from './member';

export enum TaskStatus {
  TODO = 'TODO',
  IN_PROGRESS = 'IN_PROGRESS',
  DONE = 'DONE',
}

export type Task = {
  title: string;
  assignee: Member;
  expectedSchedule: { start: Date; end: Date };
  actualSchedule: { start: Date; end: Date } | null;
  status: TaskStatus;
  expectedSize: number;
};
