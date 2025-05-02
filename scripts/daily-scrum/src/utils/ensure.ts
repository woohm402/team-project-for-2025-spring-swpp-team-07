export const ensure = <T>(data: T | undefined) => {
  if (!data) throw new Error('Data is undefined');
  return data;
};
