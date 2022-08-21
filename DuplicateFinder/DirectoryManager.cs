using DuplicateFinder.Interfaces;

namespace DuplicateFinder
{
    internal class DirectoryManager : IDirectoryManager
    {
        private readonly string _fileInput;
        private readonly string _directoryInput;
        private readonly HashSet<string> _foundMatches;
        private bool _isSearchForSpecificFile;
        private bool _fileNameWildcard;
        private bool _extensionWildcard;

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
            List<string> fullFileLocationOfMatches = new();
            _isSearchForSpecificFile = IsSearchForSpecificFile(_fileInput);
            _fileNameWildcard = UseWildcardName(_fileInput);
            _extensionWildcard = UseWildcardExtension(_fileInput);

            TraverseDirectory(_directoryInput);

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

                if (_isSearchForSpecificFile)
                {
                    if (string.Equals(fileName, _fileInput, StringComparison.OrdinalIgnoreCase))
                    {
                        _foundMatches.Add(file);
                    }
                }
                else
                {
                    if (_fileNameWildcard && _extensionWildcard)
                    {
                        _foundMatches.Add(file);
                    }
                    else if (_fileNameWildcard && !_extensionWildcard)
                    {
                        int lastIndexOfDot = fileName.LastIndexOf('.');
                        string fileExtension = fileName.Substring(lastIndexOfDot + 1);
                        int lastIndexOfDotForInputFile = _fileInput.LastIndexOf('.');
                        string inputFileExtension = _fileInput.Substring(lastIndexOfDotForInputFile + 1);

                        if (string.Equals(inputFileExtension, fileExtension, StringComparison.OrdinalIgnoreCase)) {
                            _foundMatches.Add(file);
                        }
                    }
                    else if (!_fileNameWildcard && _extensionWildcard)
                    {
                        int lastIndexOfDot = fileName.LastIndexOf('.');
                        string name = fileName.Substring(0, lastIndexOfDot);
                        int lastIndexOfDotForInputFile = _fileInput.LastIndexOf('.');

                        if (lastIndexOfDotForInputFile < 0)
                        {
                            if (string.Equals(_fileInput, name, StringComparison.OrdinalIgnoreCase))
                            {
                                _foundMatches.Add(file);
                            }

                            continue;
                        } 

                        string inputFileName = _fileInput.Substring(0 ,lastIndexOfDotForInputFile);

                        if (string.Equals(inputFileName, name, StringComparison.OrdinalIgnoreCase))
                        {
                            _foundMatches.Add(file);
                        }
                    }
                }
            }

            foreach (var folder in folders)
            {
                TraverseDirectory(folder);
            }
        }

        public void PrintDirectory(string directoryName, int tabulation = 0)
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
                PrintDirectory(folder, tabulation + 1);
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

        private bool IsSearchForSpecificFile(string inputFile)
        {
            int lastIndexOfDot = inputFile.LastIndexOf('.');

            if (lastIndexOfDot < 0)
            {
                return false;
            }

            string fileName = inputFile.Substring(0, lastIndexOfDot);
            string extension = inputFile.Substring(lastIndexOfDot + 1);

            return (fileName != "*" && extension != "*");
        }

        private bool UseWildcardExtension(string inputFile)
        {
            int lastIndexOfDot = inputFile.LastIndexOf('.');

            if (lastIndexOfDot < 0)
            {
                return true;
            }

            string extension = inputFile.Substring(lastIndexOfDot + 1);

            return extension == "*";
        }

        private bool UseWildcardName(string inputFile)
        {
            int lastIndexOfDot = inputFile.LastIndexOf('.');

            if (lastIndexOfDot < 0)
            {
                return inputFile == "*";
            }

            string fileName = inputFile.Substring(0, lastIndexOfDot);

            return fileName == "*";
        }
    }
}
