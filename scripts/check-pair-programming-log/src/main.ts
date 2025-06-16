import path from "node:path";

const sprints = [1, 2, 3, 4];

const markdownContents = await Promise.all(
  sprints.map((s) =>
    Bun.file(
      path.resolve(
        import.meta.dirname,
        `../../../reports/sprint${s}/README.md`,
      ),
    )
      .text()
      .catch(() => ""),
  ),
);

const parsed = markdownContents.flatMap((c, ci) => {
  const lines = c.split("\n");
  const logs = [];
  const lineIndex = lines.findIndex((l) => l === "## 페어 프로그래밍 기록");

  if (lineIndex === -1) return [];

  for (let i = lineIndex + 4; ; i++) {
    const line = lines.at(i);
    if (line === undefined) throw new Error();
    if (!line.startsWith("|")) break;
    logs.push(line);
  }

  return logs
    .map((l) => l.split("|").map((d) => d.trim()))
    .map(([, , driver, navigator, , time]) => {
      const [from, to] = time.split("-");
      const toMinutes = (hhmm: string) => {
        const h = parseInt(hhmm.split(":")[0]);
        const m = parseInt(hhmm.split(":")[1]);
        return h * 60 + m;
      };
      return { driver, navigator, time: toMinutes(to) - toMinutes(from) };
    });
});

console.log(
  parsed.reduce(
    (acc, cur) =>
      acc.map((a) => ({
        ...a,
        driver: a.name === cur.driver ? a.driver + cur.time : a.driver,
        navigator:
          a.name === cur.navigator ? a.navigator + cur.time : a.navigator,
      })),
    [
      { name: "우현민", driver: 0, navigator: 0 },
      { name: "곽승연", driver: 0, navigator: 0 },
      { name: "조재표", driver: 0, navigator: 0 },
      { name: "장호림", driver: 0, navigator: 0 },
      { name: "문지환", driver: 0, navigator: 0 },
    ],
  ),
);
