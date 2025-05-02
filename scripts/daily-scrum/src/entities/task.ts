import type { Member } from "./member";

export enum TaskStatus {
  TODO = "TODO",
  IN_PROGRESS = "IN_PROGRESS",
  DONE = "DONE",
}

export type Task = {
  title: string;
  assignee: Member;
  schedule: { start: Date; end: Date };
  status: TaskStatus;
};
