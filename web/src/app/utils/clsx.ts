export const clsx = (...classes: (string | null | false | undefined)[]) =>
  classes.filter((c) => typeof c === "string").join(" ");
