using DuplicateFinder.Interfaces;

namespace DuplicateFinder
{
    public class MessageWriter : IMessageWriter
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteMessages(IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                WriteMessage(message);
            }
        }
    }
}
