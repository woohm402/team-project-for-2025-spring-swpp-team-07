'use client';

import { useInterval } from '@/app/utils/useInterval';
import type { UpgradeId } from '@/entities/upgrade';
import { formatTimeMMSS } from '@/utils/time';
import Image from 'next/image';
import { memo, useReducer } from 'react';
import map from './map.png';

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
      <div className="border rounded-lg mb-4 bg-white shadow-sm transition-shadow duration-200 hover:shadow-md flex flex-col overflow-hidden">
        <button
          type="button"
          className="p-4 cursor-pointer transition-colors duration-200 hover:bg-gray-50"
          onClick={toggleDetails}
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              toggleDetails();
            }
          }}
        >
          <div className="flex justify-between items-center w-full">
            <div className="flex items-center space-x-4">
              <div className="w-9 h-9 bg-blue-500 text-white rounded-full flex items-center justify-center font-bold text-xl">
                {rank}
              </div>
              <div className="font-medium text-xl">{nickname}</div>
            </div>
            <div className="flex items-center space-x-4 pointer-events-none">
              <div className="text-xl font-semibold">{formatTimeMMSS(totalTime / 1000)}</div>
              <div className="w-6 h-6 flex items-center justify-center">
                <svg
                  className={`w-4 h-4 transform transition-transform duration-300 ${showDetails ? 'rotate-180' : ''}`}
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
          className={`overflow-hidden transition-all duration-500 grid sm:grid-cols-[1fr_360px] ease-in-out grid-cols-1 ${
            showDetails
              ? 'opacity-100 border-t border-opacity-100'
              : 'max-h-0 opacity-0 border-opacity-0 border-t'
          }`}
          aria-hidden={!showDetails}
        >
          <div className="px-4 py-3">
            <h4 className="text-sm text-gray-500 mb-3 font-bold">고른 증강 목록</h4>
            <div className="flex flex-wrap gap-2">
              {upgradeIds.map((id) => (
                <button
                  key={id}
                  className="h-10 px-2 bg-gray-100 rounded-md flex items-center justify-center text-sm font-medium transition-all duration-200 cursor-pointer border border-gray-200"
                  type="button"
                  tabIndex={0}
                  onClick={(e) => e.stopPropagation()}
                  onKeyDown={(e) => e.stopPropagation()}
                >
                  {id}
                </button>
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
  const [sliceIndex, increment] = useReducer((c) => c + 1, 0);

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
    if (normalizedLog.length === 0) return '';

    const firstPoint = normalizedLog[0];
    const restPoints = normalizedLog.slice(1);

    const moveTo = `M ${firstPoint.normalizedX},${firstPoint.normalizedY}`;
    const lineTo = restPoints
      .map((point) => `L ${point.normalizedX},${point.normalizedY}`)
      .join(' ');

    return `${moveTo} ${lineTo}`;
  })();

  return (
    <div className="relative w-[300px] h-[689px] mx-auto">
      <Image width={300} src={map} alt="" className="absolute top-0 left-0" />
      <div className="z-20 absolute inset-0">
        <svg width="300" viewBox="0 0 300 689" preserveAspectRatio="xMidYMid meet">
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
