import type { Member } from "./member";

export type PullRequest = {
  title: string;
  assignee: Member;
  reviewers: Member[];
  number: number;
  url: string;
};
