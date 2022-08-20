//string start = "C:\\Users\\Nikolay Tsvetin\\Desktop\\debug";
//DuplicateFinder finder = new(string.Empty, string.Empty);
//finder.TraverseAndPrintDirectory(start);

using DuplicateFinder;

IEnumerable<string> infoMessages = Info.GetProgramUsageInformation();
string userInputForFile = Info.GetDisplayMessageForUserInputForFile();
string userInputForDirectory = Info.GetDisplayMessageForUserInputForDirectory();

MessageWriter writer = new();
writer.WriteMessages(infoMessages);
writer.WriteMessage($"\n{userInputForFile}");

string fileInput = Console.ReadLine();
writer.WriteMessage($"\n{userInputForDirectory}");
string directoryInput = Console.ReadLine();

DirectoryManager duplicateFinder = new(fileInput, directoryInput);

try
{
    duplicateFinder.ValidateInputParameters(fileInput, directoryInput);
}
catch (ArgumentException ex)
{
    writer.WriteMessage($"\nUnable to validate input parameters: {ex.Message}");
}

List<string> matches = duplicateFinder.Search();
writer.WriteMessage("\nMatches found:");
writer.WriteMessages(matches);

Console.ReadKey();