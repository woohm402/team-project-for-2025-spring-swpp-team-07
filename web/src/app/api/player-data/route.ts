import { z } from "zod";
import mongodb from "mongodb";

export const POST = async (request: Request) => {
  const { result, data } = await (async () => {
    try {
      return {
        result: "success",
        data: z
          .object({
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
          })
          .parse(await request.json()),
      } as const;
    } catch (err) {
      return { result: "error", data: null } as const;
    }
  })();

  if (result === "error") return new Response(null, { status: 400 });

  const mongodbUrl = `mongodb+srv://woohm404:${process.env.MONGODB_PASSWORD}@cluster0.qtwt5z8.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0`;

  const client = await mongodb.MongoClient.connect(mongodbUrl);

  try {
    const db = client.db("player-data");
    const collection = db.collection("players");
    await collection.insertOne(data);
  } finally {
    await client.close();
  }

  return new Response(null, { status: 201 });
};
