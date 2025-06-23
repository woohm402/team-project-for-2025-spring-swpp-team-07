export const UPGRADE_IDS = [
  101, 102, 103, 104, 105, 201, 202, 203, 204, 301, 302, 303, 401, 402, 403,
  404, 501, 502, 503,
] as const;

export type UpgradeId = (typeof UPGRADE_IDS)[number];
