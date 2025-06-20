/**
 * Formats time in seconds to MM:SS format
 * @param timeInSeconds Time in seconds
 * @returns Formatted time string in MM:SS format
 */
export const formatTimeMMSS = (timeInSeconds: number): string => {
  const minutes = Math.floor(timeInSeconds / 60);
  const seconds = Math.floor(timeInSeconds % 60);
  return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
};

/**
 * Formats time in seconds to MM:SS.MS format with milliseconds
 * @param timeInSeconds Time in seconds
 * @returns Formatted time string in MM:SS.MS format
 */
export const formatTimeMMSSMS = (timeInSeconds: number): string => {
  const minutes = Math.floor(timeInSeconds / 60);
  const seconds = Math.floor(timeInSeconds % 60);
  const milliseconds = Math.floor((timeInSeconds % 1) * 1000);
  return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}.${milliseconds.toString().padStart(3, '0')}`;
};

/**
 * Formats time in seconds to HH:MM:SS format
 * @param timeInSeconds Time in seconds
 * @returns Formatted time string in HH:MM:SS format
 */
export const formatTimeHHMMSS = (timeInSeconds: number): string => {
  const hours = Math.floor(timeInSeconds / 3600);
  const minutes = Math.floor((timeInSeconds % 3600) / 60);
  const seconds = Math.floor(timeInSeconds % 60);
  return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
};
