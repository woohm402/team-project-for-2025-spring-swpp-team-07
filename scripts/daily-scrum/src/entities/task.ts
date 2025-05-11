import type { Member } from './member';

export type Task = {
  title: string;
  assignee: Member;
  expectedSchedule: { start: Date; end: Date };
  actualSchedule: { start: Date; end: Date } | null;
  skip: boolean;
  expectedSize: number;
};
