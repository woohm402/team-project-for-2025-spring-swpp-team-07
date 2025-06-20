'use client';

import type { UpgradeId } from '@/entities/upgrade';
import { formatTimeMMSS } from '@/utils/time';
import { memo, useCallback, useMemo, useState } from 'react';

// CSS 클래스 상수 정의
const CLASSES = {
  container: 'border rounded-lg mb-4 bg-white shadow-sm transition-shadow duration-200',
  containerHover: 'hover:shadow-md',
  clickArea: 'p-4 cursor-pointer transition-colors duration-200 hover:bg-gray-50',
  header: 'flex justify-between items-center w-full',
  rankBadge:
    'w-9 h-9 bg-blue-500 text-white rounded-full flex items-center justify-center font-bold text-xl',
  detailsContainer: 'overflow-hidden transition-all duration-500 ease-in-out',
  detailsVisible: 'max-h-[700px] opacity-100 border-t border-opacity-100',
  detailsHidden: 'max-h-0 opacity-0 border-opacity-0 border-t',
  sectionHeading: 'text-sm text-gray-500 mb-3',
  sectionContainer: 'px-4 py-3',
  upgradeContainer: 'flex flex-wrap gap-2',
  upgradeBadge:
    'min-w-[45px] h-10 px-2 bg-gray-100 rounded-md flex items-center justify-center text-sm font-medium transition-all duration-200 cursor-pointer border border-gray-200',
  pathContainer: 'border rounded-md p-2 bg-gray-50 mt-3',
  noDataMessage: 'text-center py-4 text-gray-400',
  chevron: 'w-4 h-4 transform transition-transform duration-300 will-change-transform',
  chevronRotated: 'rotate-180',
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
    const [showDetails, setShowDetails] = useState(false);
    const toggleDetails = useCallback(() => setShowDetails((prev) => !prev), []);

    // Normalize positions for the path visualization
    const normalizedLog = useMemo(() => {
      if (log.length === 0) return [];

      // Find min/max to normalize coordinates using reduce
      const bounds = log.reduce(
        (acc, entry) => ({
          minX: Math.min(acc.minX, entry.position.x),
          minY: Math.min(acc.minY, entry.position.y),
          maxX: Math.max(acc.maxX, entry.position.x),
          maxY: Math.max(acc.maxY, entry.position.y),
        }),
        {
          minX: Number.POSITIVE_INFINITY,
          minY: Number.POSITIVE_INFINITY,
          maxX: Number.NEGATIVE_INFINITY,
          maxY: Number.NEGATIVE_INFINITY,
        },
      );

      // Width and height for the SVG
      const width = 300;
      const height = 300;

      // Calculate scale to fit the path in the SVG
      const scaleX = width / (bounds.maxX - bounds.minX || 1); // avoid division by zero
      const scaleY = height / (bounds.maxY - bounds.minY || 1);

      // Normalize positions
      return log.map((entry) => ({
        ...entry,
        normalizedX: (entry.position.x - bounds.minX) * scaleX,
        normalizedY: (entry.position.y - bounds.minY) * scaleY,
      }));
    }, [log]);

    // Generate SVG path
    const pathData = useMemo(() => {
      if (normalizedLog.length === 0) return '';

      const firstPoint = normalizedLog[0];
      const restPoints = normalizedLog.slice(1);

      const moveTo = `M ${firstPoint.normalizedX},${firstPoint.normalizedY}`;
      const lineTo = restPoints
        .map((point) => `L ${point.normalizedX},${point.normalizedY}`)
        .join(' ');

      return `${moveTo} ${lineTo}`;
    }, [normalizedLog]);

    return (
      <div
        className={`${CLASSES.container} ${CLASSES.containerHover} flex flex-col overflow-hidden`}
      >
        <button
          type="button"
          className={`${CLASSES.clickArea}`}
          onClick={toggleDetails}
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              toggleDetails();
            }
          }}
        >
          <div className={CLASSES.header}>
            <div className="flex items-center space-x-4">
              <div className={CLASSES.rankBadge}>{rank}</div>
              <div className="font-medium text-xl">{nickname}</div>
            </div>
            <div className="flex items-center space-x-4 pointer-events-none">
              <div className="text-xl font-semibold">{formatTimeMMSS(totalTime / 1000)}</div>
              <div className="w-6 h-6 flex items-center justify-center">
                <svg
                  className={`${CLASSES.chevron} ${showDetails ? CLASSES.chevronRotated : ''}`}
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
          className={`${CLASSES.detailsContainer} ${
            showDetails ? CLASSES.detailsVisible : CLASSES.detailsHidden
          }`}
          style={{ willChange: 'max-height, opacity' }}
          aria-hidden={!showDetails}
        >
          <div className={CLASSES.sectionContainer}>
            <h4 className={CLASSES.sectionHeading}>Upgrades</h4>
            <div className={CLASSES.upgradeContainer}>
              {upgradeIds.map((id) => (
                <button
                  key={id}
                  className={CLASSES.upgradeBadge}
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

          <div className={CLASSES.sectionContainer}>
            <h4 className={CLASSES.sectionHeading}>Movement Path</h4>
            <div className={CLASSES.pathContainer}>
              {normalizedLog.length === 0 ? (
                <div className={CLASSES.noDataMessage}>No movement data available</div>
              ) : (
                <svg
                  width="100%"
                  height="250"
                  viewBox="0 0 300 250"
                  preserveAspectRatio="xMidYMid meet"
                  style={{ willChange: 'transform' }}
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
              )}
            </div>
          </div>
        </div>
      </div>
    );
  },
);
