import { z } from "zod";

const playerDataSchema = z.object({
  nickname: z.string(),
  totalTime: z.number(),
  augmentIds: z.array(z.number()),
  logs: z.array(
    z.object({
      time: z.number(),
      x: z.number(),
      y: z.number(),
      z: z.number(),
    }),
  ),
});

export type PlayerData = z.infer<typeof playerDataSchema>;
export const parsePlayerData = playerDataSchema.parse;
