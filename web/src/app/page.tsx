import type { PlayerData } from '@/entities/player-data';
import { UPGRADE_IDS } from '@/entities/upgrade';
import { ensure } from '@/utils/ensure';
import { MongoClient, type ObjectId, ServerApiVersion } from 'mongodb';
import { Item } from './_components/Item';

export const dynamic = 'force-dynamic';

export default async function Home() {
  const mongodbUrl = `mongodb+srv://woohm404:${process.env.MONGODB_PASSWORD}@cluster0.qtwt5z8.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0`;
  const client = await MongoClient.connect(mongodbUrl, {
    serverApi: ServerApiVersion.v1,
  });

  const data = (await client
    .db('player-data')
    .collection('players')
    .find()
    .sort({ totalTime: 1 })
    .limit(10)
    .toArray()) as ({ _id: ObjectId } & PlayerData)[];

  return (
    <ul className="flex flex-col py-12 px-6">
      {data.map((item, i) => (
        <li key={item._id.toString()}>
          <Item
            log={item.logs.map((l) => ({
              time: l.time,
              position: { x: l.x, y: l.z },
            }))}
            nickname={item.nickname}
            rank={i + 1}
            totalTime={item.totalTime}
            upgradeIds={item.augmentIds.map((id) => ensure(UPGRADE_IDS.find((i) => i === id)))}
          />
        </li>
      ))}
    </ul>
  );
}
