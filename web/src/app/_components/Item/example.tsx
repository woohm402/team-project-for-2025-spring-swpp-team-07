import React from "react";
import { Item } from "./index";

export const ItemExample = () => {
  // Mock data
  const raceRecord = {
    rank: 1,
    nickname: "SpeedKing",
    totalTime: 125.4, // 2:05.4
    upgradeIds: [101, 202, 303, 404],
    log: [
      { time: 0, position: { x: 10, y: 10 } },
      { time: 10, position: { x: 30, y: 20 } },
      { time: 20, position: { x: 50, y: 40 } },
      { time: 30, position: { x: 60, y: 70 } },
      { time: 40, position: { x: 70, y: 90 } },
      { time: 50, position: { x: 90, y: 100 } },
      { time: 60, position: { x: 120, y: 110 } },
      { time: 70, position: { x: 150, y: 115 } },
      { time: 80, position: { x: 180, y: 118 } },
      { time: 90, position: { x: 210, y: 120 } },
      { time: 100, position: { x: 240, y: 118 } },
      { time: 110, position: { x: 270, y: 115 } },
      { time: 120, position: { x: 300, y: 110 } },
    ],
  };

  // Simulate leaderboard with multiple items
  const leaderboard = [
    raceRecord,
    {
      rank: 2,
      nickname: "RaceMaster",
      totalTime: 128.7,
      upgradeIds: [101, 104, 204, 302],
      log: [
        { time: 0, position: { x: 10, y: 10 } },
        { time: 20, position: { x: 40, y: 30 } },
        { time: 40, position: { x: 70, y: 60 } },
        { time: 60, position: { x: 100, y: 80 } },
        { time: 80, position: { x: 130, y: 90 } },
        { time: 100, position: { x: 160, y: 85 } },
        { time: 120, position: { x: 190, y: 75 } },
      ],
    },
    {
      rank: 3,
      nickname: "DriftKing",
      totalTime: 131.2,
      upgradeIds: [102, 201, 301, 401],
      log: [
        { time: 0, position: { x: 10, y: 10 } },
        { time: 25, position: { x: 50, y: 20 } },
        { time: 50, position: { x: 90, y: 50 } },
        { time: 75, position: { x: 130, y: 90 } },
        { time: 100, position: { x: 170, y: 70 } },
        { time: 125, position: { x: 210, y: 40 } },
      ],
    },
  ];

  return (
    <div className="max-w-2xl mx-auto p-4">
      <h2 className="text-2xl font-bold mb-6">Race Leaderboard</h2>
      {leaderboard.map((record, index) => (
        <Item
          key={index}
          rank={record.rank}
          nickname={record.nickname}
          totalTime={record.totalTime}
          upgradeIds={record.upgradeIds}
          log={record.log}
        />
      ))}
    </div>
  );
};