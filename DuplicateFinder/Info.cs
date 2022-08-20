namespace DuplicateFinder
{
    internal static class Info
    {
        internal static IEnumerable<string> GetProgramUsageInformation()
        {
            return new List<string>()
            {
                "Welcome to deduplication tool for file system.",
                "\n1. You can choose between the following search options:",
                "\t1.1 Specific file name, example: \"test\"",
                "\t1.2 Full file name, example: \"test.txt\"",
                "\t1.3 Wild cards, example: \"test.*\" or \"*.txt\"",
                "\n2. Your can select your search to be in:",
                "\t2.1 Specific directory, example: \"C:\\Users\\Nikolay Tsvetin\\Desktop\\debug\"",
                "\t2.2 \"*\" - Whole file system"
            };
        }

        internal static string GetDisplayMessageForUserInputForFile() => "Please type the file you want to search based on the options mentioned in point 1 above:";

        internal static string GetDisplayMessageForUserInputForDirectory() => "Please type the directory you want to be search in based on the options mentioned in point 2 above:";
    }
}
