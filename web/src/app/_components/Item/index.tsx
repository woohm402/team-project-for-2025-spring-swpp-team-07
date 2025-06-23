"use client";

import upgrade101 from "./upgrades/101.png";
import upgrade104 from "./upgrades/104.png";
import upgrade202 from "./upgrades/202.png";
import upgrade301 from "./upgrades/301.png";
import upgrade401 from "./upgrades/401.png";
import upgrade404 from "./upgrades/404.png";
import upgrade503 from "./upgrades/503.png";
import upgrade102 from "./upgrades/102.png";
import upgrade105 from "./upgrades/105.png";
import upgrade203 from "./upgrades/203.png";
import upgrade302 from "./upgrades/302.png";
import upgrade402 from "./upgrades/402.png";
import upgrade501 from "./upgrades/501.png";
import upgrade103 from "./upgrades/103.png";
import upgrade201 from "./upgrades/201.png";
import upgrade204 from "./upgrades/204.png";
import upgrade303 from "./upgrades/303.png";
import upgrade403 from "./upgrades/403.png";
import upgrade502 from "./upgrades/502.png";
import { useInterval } from "@/app/utils/useInterval";
import type { UpgradeId } from "@/entities/upgrade";
import { formatTimeMMSS } from "@/utils/time";
import Image, { StaticImageData } from "next/image";
import { memo, useReducer } from "react";
import map from "./map.png";

const upgradeIdImageMap: Record<UpgradeId, StaticImageData> = {
  101: upgrade101,
  102: upgrade102,
  103: upgrade103,
  104: upgrade104,
  105: upgrade105,
  201: upgrade201,
  202: upgrade202,
  203: upgrade203,
  204: upgrade204,
  301: upgrade301,
  302: upgrade302,
  303: upgrade303,
  401: upgrade401,
  402: upgrade402,
  403: upgrade403,
  404: upgrade404,
  501: upgrade501,
  502: upgrade502,
  503: upgrade503,
};

export const Item = memo(
  ({
    rank,
    nickname,
    totalTime,
    upgradeIds,
    log,
  }: {
    rank: number;
    nickname: string;
    totalTime: number; // in milliseconds
    upgradeIds: UpgradeId[];
    log: { time: number; position: { x: number; y: number } }[];
  }) => {
    const [showDetails, toggleDetails] = useReducer((t) => !t, false);

    return (
      <div className="border rounded-lg mb-4 shadow-sm transition-shadow duration-200 hover:shadow-md flex flex-col overflow-hidden">
        <button
          type="button"
          className="p-4 cursor-pointer transition-colors duration-200 hover:bg-white bg-white/80"
          onClick={() => {
            toggleDetails();
          }}
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === "Enter" || e.key === " ") {
              e.preventDefault();
              toggleDetails();
            }
          }}
        >
          <div className="flex justify-between items-center w-full">
            <div className="flex items-center space-x-4">
              <div className="w-9 h-9 flex items-center justify-center font-bold text-white bg-black/80 rounded-full text-sm">
                {rank}
              </div>
              <div className="font-medium text-xl">{nickname}</div>
            </div>
            <div className="flex items-center space-x-4 pointer-events-none">
              <div className="text-xl font-semibold">
                {formatTimeMMSS(totalTime / 1000)}
              </div>
              <div className="w-6 h-6 flex items-center justify-center">
                <svg
                  className={`w-4 h-4 transform transition-transform duration-300 ${showDetails ? "rotate-180" : ""}`}
                  aria-hidden="true"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M19 9l-7 7-7-7"
                  />
                </svg>
              </div>
            </div>
          </div>
        </button>

        <div
          className={`bg-white/80 overflow-hidden transition-all duration-500 grid sm:grid-cols-[1fr_360px] ease-in-out grid-cols-1 ${
            showDetails
              ? "opacity-100 border-t border-opacity-100"
              : "max-h-0 opacity-0 border-opacity-0 border-t"
          }`}
          aria-hidden={!showDetails}
        >
          <div className="px-4 py-3">
            <h4 className="text-sm text-gray-500 mb-3 font-bold">
              고른 증강 목록
            </h4>
            <div className="flex flex-wrap gap-2">
              {upgradeIds.map((id) => (
                <Image
                  src={upgradeIdImageMap[id]}
                  key={id}
                  alt={id.toString()}
                  className="w-40 px-2 bg-gray-100 rounded-md flex items-center justify-center text-sm font-medium transition-all duration-200 cursor-pointer border border-gray-200"
                />
              ))}
            </div>
          </div>

          <div className="px-4 py-3">
            <h4 className="text-sm text-gray-500 mb-3 font-bold">이동 경로</h4>
            <div className="border rounded-md p-2 bg-gray-50 mt-3">
              {showDetails ? <MovePath logs={log} /> : null}
            </div>
          </div>
        </div>
      </div>
    );
  },
);

const MovePath = ({
  logs: log,
}: {
  logs: { time: number; position: { x: number; y: number } }[];
}) => {
  const [sliceIndex, increment] = useReducer((c) => c + 3, 0);

  useInterval(() => {
    if (sliceIndex < log.length) increment();
  }, 50);

  // Normalize positions for the path visualization
  const normalizedLog = (() => {
    const minX = -1480;
    const minY = -3730;
    const height = 689;

    const scale = 0.115;

    // Normalize positions
    return log
      .map((entry) => ({
        ...entry,
        normalizedX: (entry.position.x - minX) * scale,
        normalizedY: height - (entry.position.y - minY) * scale,
      }))
      .slice(0, sliceIndex);
  })();

  // Generate SVG path
  const pathData = (() => {
    if (normalizedLog.length === 0) return "";

    const firstPoint = normalizedLog[0];
    const restPoints = normalizedLog.slice(1);

    const moveTo = `M ${firstPoint.normalizedX},${firstPoint.normalizedY}`;
    const lineTo = restPoints
      .map((point) => `L ${point.normalizedX},${point.normalizedY}`)
      .join(" ");

    return `${moveTo} ${lineTo}`;
  })();

  return (
    <div className="relative w-[300px] h-[689px] mx-auto">
      <Image width={300} src={map} alt="" className="absolute top-0 left-0" />
      <div className="z-20 absolute inset-0">
        <svg
          width="300"
          viewBox="0 0 300 689"
          preserveAspectRatio="xMidYMid meet"
        >
          <title>map</title>
          <path
            d={pathData}
            stroke="#3b82f6"
            strokeWidth="2"
            fill="none"
            strokeLinejoin="round"
            strokeLinecap="round"
          />
          {/* Start point */}
          <circle
            cx={normalizedLog[0]?.normalizedX}
            cy={normalizedLog[0]?.normalizedY}
            r="4"
            fill="green"
          />
          {/* End point */}
          <circle
            cx={normalizedLog[normalizedLog.length - 1]?.normalizedX}
            cy={normalizedLog[normalizedLog.length - 1]?.normalizedY}
            r="4"
            fill="red"
          />
        </svg>
      </div>
    </div>
  );
};
