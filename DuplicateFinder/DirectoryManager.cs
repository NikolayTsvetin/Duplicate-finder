using DuplicateFinder.Interfaces;

namespace DuplicateFinder
{
    internal class DirectoryManager : IDirectoryManager
    {
        private readonly string _fileInput;
        private readonly string _directoryInput;
        private readonly HashSet<string> _foundMatches;

        public DirectoryManager(string fileInput, string directoryInput)
        {
            _fileInput = fileInput;
            _directoryInput = directoryInput;
            _foundMatches = new();
        }

        public void ValidateInputParameters(string inputFile, string inputDirectory)
        {
            if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(inputDirectory))
            {
                throw new ArgumentException("Input parameter is null or empty.", string.IsNullOrEmpty(inputFile) ? nameof(inputFile) : nameof(inputDirectory));
            }

            if (!Directory.Exists(inputDirectory) && inputDirectory != "*")
            {
                throw new ArgumentException($"Directory {inputDirectory} does not exist.");
            }
        }

        public List<string> Search()
        {
            TraverseDirectory(_directoryInput);

            List<string> fullFileLocationOfMatches = new();

            foreach (string match in _foundMatches)
            {
                fullFileLocationOfMatches.Add(match);
            }

            return fullFileLocationOfMatches;
        }

        public void TraverseDirectory(string directoryName)
        {
            string[] files = Directory.GetFiles(directoryName);
            string[] folders = Directory.GetDirectories(directoryName);

            if (files == null || files.Length == 0)
            {
                return;
            }

            foreach (string file in files)
            {
                string[] splittedFileName = file.Split('\\');
                string fileName = splittedFileName[splittedFileName.Length - 1];

                // TODO: Case insensitive parameter? Wildcards?
                if (fileName == _fileInput)
                {
                    _foundMatches.Add(file);
                }
            }

            foreach (var folder in folders)
            {
                TraverseDirectory(folder);
            }
        }

        public void TraverseAndPrintDirectory(string directoryName, int tabulation = 0)
        {
            Dictionary<string, string> tabulationResult = CalculateTabulation(tabulation);

            string[] splitted = directoryName.Split('\\');
            Console.WriteLine($"{tabulationResult["directoryTabulation"]}In directory: {splitted[splitted.Length - 1]}");

            string[] files = Directory.GetFiles(directoryName);
            string[] folders = Directory.GetDirectories(directoryName);

            if (files == null || files.Length == 0)
            {
                Console.WriteLine($"{tabulationResult["filesTabulation"]}Directory: {splitted[splitted.Length - 1]} is empty.");

                return;
            }

            foreach (string file in files)
            {
                string[] splittedFileName = file.Split('\\');

                Console.WriteLine($"{tabulationResult["filesTabulation"]}In {splitted[splitted.Length - 1]} found: {splittedFileName[splittedFileName.Length - 1]}");
            }

            foreach (var folder in folders)
            {
                TraverseAndPrintDirectory(folder, tabulation + 1);
            }
        }

        private Dictionary<string, string> CalculateTabulation(int tabulation)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string directoryTabulation = string.Empty;
            string filesTabulation = string.Empty;

            for (int i = 0; i < tabulation; i++)
            {
                directoryTabulation += "\t";
            }

            for (int i = 0; i < tabulation + 1; i++)
            {
                filesTabulation += "\t";
            }

            result.Add("directoryTabulation", directoryTabulation);
            result.Add("filesTabulation", filesTabulation);

            return result;
        }
    }
}
