namespace DuplicateFinder.Interfaces
{
    internal interface IDirectoryManager
    {
        internal void ValidateInputParameters(string inputFile, string inputDirectory);

        internal List<string> Search();

        internal void TraverseDirectory(string directoryName);
    }
}
