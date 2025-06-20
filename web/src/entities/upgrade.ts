export const UPGRADE_IDS = [
  101, 102, 103, 104, 105, 201, 202, 203, 204, 205, 301, 302, 303, 401, 402, 403, 404,
] as const;

export type UpgradeId = (typeof UPGRADE_IDS)[number];
