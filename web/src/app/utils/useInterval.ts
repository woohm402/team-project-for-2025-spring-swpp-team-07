import { useEffect, useRef } from 'react';

export const useInterval = (callback: () => void, delay: number) => {
  const callbackRef = useRef<() => void>(null);

  useEffect(() => {
    callbackRef.current = callback;
  }, [callback]);

  useEffect(() => {
    const interval = setInterval(() => callbackRef.current?.(), delay);

    return () => {
      clearInterval(interval);
    };
  }, [delay]);
};
