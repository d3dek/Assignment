using FileUtilities;

var generator = new FileGenerator();
var sorter = new FileSorter();
var mainProgramLoop = new Main(generator, sorter);
await mainProgramLoop.Run(args);