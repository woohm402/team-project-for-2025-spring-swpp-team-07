import path from 'node:path';
import { mdToPdf } from 'md-to-pdf';
import { z } from 'zod';

const sprint = z.enum(['1', '2', '3', '4']).parse(process.argv[2], {
  errorMap: () => ({ message: '1, 2, 3, 4 중 하나여야 합니다.' }),
});

const rootDirectory = path.resolve(import.meta.dirname, '../../..');
const sprintDirectory = path.resolve(rootDirectory, `reports/sprint${sprint}`);
const outputDirectory = path.resolve(sprintDirectory, 'output');
const filename = `SWPP_team07_sprint${sprint}`;
const reportMarkdownPath = path.resolve(sprintDirectory, 'README.md');

if (!(await Bun.file(reportMarkdownPath).exists())) {
  console.error(`스프린트 ${sprint} 리포트 파일이 없습니다.`);
  process.exit(1);
}

const pdf = await mdToPdf({ path: reportMarkdownPath });

const pdfPath = path.resolve(outputDirectory, `${filename}.pdf`);
await Bun.file(pdfPath).write(pdf.content);
await Bun.$`zip -j ${path.resolve(outputDirectory, `${filename}.zip`)} ${pdfPath}`.quiet();

console.info('리포트가 생성되었습니다.');
console.info(path.resolve(outputDirectory, `${filename}.zip`));
