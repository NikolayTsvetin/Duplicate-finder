namespace DuplicateFinder.Interfaces
{
    internal interface IMessageWriter
    {
        internal void WriteMessage(string message);

        internal void WriteMessages(IEnumerable<string> messages);
    }
}
