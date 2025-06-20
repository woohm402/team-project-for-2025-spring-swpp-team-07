import { MongoClient } from "mongodb";
import { parsePlayerData } from "@/entities/player-data";

export const POST = async (request: Request) => {
  const { result, data } = await (async () => {
    try {
      return {
        result: "success",
        data: parsePlayerData(await request.json()),
      } as const;
    } catch (err) {
      return { result: "error", data: null } as const;
    }
  })();

  if (result === "error") return new Response(null, { status: 400 });

  const mongodbUrl = `mongodb+srv://woohm404:${process.env.MONGODB_PASSWORD}@cluster0.qtwt5z8.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0`;

  const client = await MongoClient.connect(mongodbUrl);

  try {
    const db = client.db("player-data");
    const collection = db.collection("players");
    await collection.insertOne(data);
  } finally {
    await client.close();
  }

  return new Response(null, { status: 201 });
};
